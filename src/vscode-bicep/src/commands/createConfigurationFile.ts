// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { window, Uri, workspace } from "vscode";
import { Command } from "./types";
import { LanguageClient } from "vscode-languageclient/node";
import {
  IActionContext,
  parseError,
  UserCancelledError,
} from "@microsoft/vscode-azext-utils";
import path from "path";
import * as fse from "fs-extra";
import {
  BicepGetRecommendedConfigLocationResult,
  createBicepConfigRequestType,
  getRecommendedConfigLocationRequestType,
} from "../language/protocol";
import { getLogger } from "../utils/logger";

const bicepConfig = "bicepconfig.json";

export class CreateBicepConfigurationFile implements Command {
  public readonly id = "bicep.createConfigFile";

  public constructor(private readonly client: LanguageClient) {}

  public async execute(
    _context: IActionContext,
    documentUri?: Uri,
    suppressQuery?: boolean, // If true, the recommended location is used without querying user (for testing)
    rethrow?: boolean // (for testing)
  ): Promise<string | undefined> {
    getLogger().debug(`asdfg20: ${Uri.toString()}`);

    // eslint-disable-next-line no-debugger
    debugger;
    console.warn("console.warn");
    console.error("console.error");
    console.log("console.log");
    getLogger().debug("getLogger().debug()");
    getLogger().warn("getLogger().warn()");
    getLogger().error("getLogger().error()");

    _context.errorHandling.rethrow = !!rethrow;

    documentUri ??= window.activeTextEditor?.document.uri;
    getLogger().debug(`asdfg21: ${String(documentUri?.toString())}`);

    const recommendation: BicepGetRecommendedConfigLocationResult =
      await this.client.sendRequest(getRecommendedConfigLocationRequestType, {
        BicepFilePath: documentUri?.fsPath,
      });
    if (recommendation.error || !recommendation.recommendedFolder) {
      throw new Error(
        `Could not determine recommended configuration location: ${
          recommendation.error ?? "Unknown"
        }`
      );
    }
    getLogger().debug(`asdfg22: ${recommendation.recommendedFolder}`);

    let selectedPath: string = path.join(
      recommendation.recommendedFolder,
      bicepConfig
    );
    getLogger().debug(`asdfg23: ${selectedPath}`);
    if (!suppressQuery) {
      // eslint-disable-next-line no-constant-condition
      while (true) {
        const response = await window.showSaveDialog({
          defaultUri: Uri.file(selectedPath),
          filters: { "bicep.config files": [bicepConfig] },
          title: "Where would you like to save the Bicep configuration file?",
          saveLabel: "Save configuration file",
        });
        if (!response || !response.fsPath) {
          throw new UserCancelledError("browse");
        }

        selectedPath = response.fsPath;

        if (path.basename(selectedPath) !== bicepConfig) {
          window.showErrorMessage(
            `A Bicep configuration file must be named ${bicepConfig}`
          );
          selectedPath = path.join(path.dirname(selectedPath), bicepConfig);
        } else {
          break;
        }
      }
    }
    getLogger().debug(`asdfg24: ${selectedPath}`);

    // eslint-disable-next-line no-debugger
    debugger;
    getLogger().debug(`selectedPath: ${selectedPath}`);
    let p = selectedPath;
    while (path.dirname(p) !== p) {
      try {
        getLogger().debug(`${p}:`);
      } catch (err) {
        getLogger().error(parseError(err).message);
      }
      try {
        getLogger().debug(`  exists: ${fse.existsSync(p)}`);
      } catch (err) {
        getLogger().error(parseError(err).message);
      }
      try {
        getLogger().debug(`  dir: ${fse.readdirSync(p).join(" | ")}`);
      } catch (err) {
        getLogger().error(parseError(err).message);
      }
      p = path.dirname(p);
    }

    getLogger().debug(`asdfg1`);
    await this.client.sendRequest(createBicepConfigRequestType, {
      destinationPath: selectedPath,
    });
    getLogger().debug(`asdfg2`);

    if (await fse.pathExists(selectedPath)) {
      getLogger().debug(`asdfg3`);
      const textDocument = await workspace.openTextDocument(selectedPath);
      getLogger().debug(`asdfg4`);
      await window.showTextDocument(textDocument);
      getLogger().debug(`asdfg5`);
      return selectedPath;
    } else {
      getLogger().debug(`asdfg6`);
      throw new Error(
        "Configuration file was not created by the language server"
      );
    }

    getLogger().debug(`asdfg7`);
  }
}

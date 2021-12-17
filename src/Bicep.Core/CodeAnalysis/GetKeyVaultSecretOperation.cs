// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public class GetKeyVaultSecretOperation : Operation
    {
        public GetKeyVaultSecretOperation(ResourceIdOperation keyVaultId, Operation secretName, Operation? secretVersion)
        {
            KeyVaultId = keyVaultId;
            SecretName = secretName;
            SecretVersion = secretVersion;
        }

        public ResourceIdOperation KeyVaultId { get; }

        public Operation SecretName { get; }

        public Operation? SecretVersion { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitGetKeyVaultSecretOperation(this);
    }
}

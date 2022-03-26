// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.UnitTests.Assertions;
using Bicep.Core.UnitTests.Utils;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bicep.Core.IntegrationTests
{
    /// <summary>
    /// Fixing https://github.com/Azure/bicep/issues/6075 required major changes to how name expressions and classic dependencies
    /// are emitted, so that complexity warrants adding a special test class for the entire issue.
    /// </summary>
    [TestClass]
    public class Issue6075Tests
    {
        [TestMethod]
        public void ThreeNestedResources_AllIndexVariables()
        {
            var result = CompilationHelper.Compile(@"
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = [for (ii, i) in range(0, 2): {
  name: string(i)
}]

resource subnet 'Microsoft.Network/virtualNetworks/subnets@2021-05-01' = [for (jj, j) in range(0, 6): {
  parent: vnet[j % 2]
  name: string(j)
}]

resource thing 'Microsoft.Network/virtualNetworks/subnets/things@2021-05-01' = [for (kk, k) in range(0, 24): {
  parent: subnet[k % 6]
  name: string(k)
}]
");
            result.Should().GenerateATemplate();

            var template = result.Template;
            using (new AssertionScope())
            {
                template.Should().HaveValueAtPath("$.resources[0].copy.name", "vnet");
                template.Should().HaveValueAtPath("$.resources[0].name", "[string(copyIndex())]");
                template.Should().NotHaveValueAtPath("$.resources[0].dependsOn");

                template.Should().HaveValueAtPath("$.resources[1].copy.name", "subnet");
                template.Should().HaveValueAtPath("$.resources[1].name", "[format('{0}/{1}', string(mod(copyIndex(), 2)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[1].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks', string(mod(copyIndex(), 2)))]"));

                template.Should().HaveValueAtPath("$.resources[2].copy.name", "thing");
                template.Should().HaveValueAtPath("$.resources[2].name", "[format('{0}/{1}/{2}', string(mod(mod(copyIndex(), 6), 2)), string(mod(copyIndex(), 6)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[2].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks/subnets', string(mod(mod(copyIndex(), 6), 2)), string(mod(copyIndex(), 6)))]"));
            }
        }

        [TestMethod]
        public void ThreeNestedResources_TopItemVariable()
        {
            var result = CompilationHelper.Compile(@"
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = [for (ii, i) in range(0, 2): {
  name: string(ii)
}]

resource subnet 'Microsoft.Network/virtualNetworks/subnets@2021-05-01' = [for (jj, j) in range(0, 6): {
  parent: vnet[j % 2]
  name: string(j)
}]

resource thing 'Microsoft.Network/virtualNetworks/subnets/things@2021-05-01' = [for (kk, k) in range(0, 24): {
  parent: subnet[k % 6]
  name: string(k)
}]
");
            result.Should().GenerateATemplate();

            var template = result.Template;
            using (new AssertionScope())
            {
                template.Should().HaveValueAtPath("$.resources[0].copy.name", "vnet");
                template.Should().HaveValueAtPath("$.resources[0].name", "[string(range(0, 2)[copyIndex()])]");
                template.Should().NotHaveValueAtPath("$.resources[0].dependsOn");

                template.Should().HaveValueAtPath("$.resources[1].copy.name", "subnet");
                template.Should().HaveValueAtPath("$.resources[1].name", "[format('{0}/{1}', string(range(0, 2)[mod(copyIndex(), 2)]), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[1].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks', string(range(0, 2)[mod(copyIndex(), 2)]))]"));

                template.Should().HaveValueAtPath("$.resources[2].copy.name", "thing");
                template.Should().HaveValueAtPath("$.resources[2].name", "[format('{0}/{1}/{2}', string(range(0, 2)[mod(mod(copyIndex(), 6), 2)]), string(mod(copyIndex(), 6)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[2].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks/subnets', string(range(0, 2)[mod(mod(copyIndex(), 6), 2)]), string(mod(copyIndex(), 6)))]"));
            }
        }

        [TestMethod]
        public void ThreeNestedResources_MiddleItemVariable()
        {
            var result = CompilationHelper.Compile(@"
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = [for (ii, i) in range(0, 2): {
  name: string(i)
}]

resource subnet 'Microsoft.Network/virtualNetworks/subnets@2021-05-01' = [for (jj, j) in range(0, 6): {
  parent: vnet[jj % 2]
  name: string(j)
}]

resource thing 'Microsoft.Network/virtualNetworks/subnets/things@2021-05-01' = [for (kk, k) in range(0, 24): {
  parent: subnet[k % 6]
  name: string(k)
}]
");
            result.Should().GenerateATemplate();

            var template = result.Template;
            using (new AssertionScope())
            {
                template.Should().HaveValueAtPath("$.resources[0].copy.name", "vnet");
                template.Should().HaveValueAtPath("$.resources[0].name", "[string(copyIndex())]");
                template.Should().NotHaveValueAtPath("$.resources[0].dependsOn");

                template.Should().HaveValueAtPath("$.resources[1].copy.name", "subnet");
                template.Should().HaveValueAtPath("$.resources[1].name", "[format('{0}/{1}', string(mod(range(0, 6)[copyIndex()], 2)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[1].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks', string(mod(range(0, 6)[copyIndex()], 2)))]"));

                template.Should().HaveValueAtPath("$.resources[2].copy.name", "thing");
                template.Should().HaveValueAtPath("$.resources[2].name", "[format('{0}/{1}/{2}', string(mod(range(0, 6)[mod(copyIndex(), 6)], 2)), string(mod(copyIndex(), 6)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[2].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks/subnets', string(mod(range(0, 6)[mod(copyIndex(), 6)], 2)), string(mod(copyIndex(), 6)))]"));
            }
        }

        [TestMethod]
        public void ThreeNestedResources_BottomItemVariable()
        {
            var result = CompilationHelper.Compile(@"
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = [for (ii, i) in range(0, 2): {
  name: string(i)
}]

resource subnet 'Microsoft.Network/virtualNetworks/subnets@2021-05-01' = [for (jj, j) in range(0, 6): {
  parent: vnet[j % 2]
  name: string(j)
}]

resource thing 'Microsoft.Network/virtualNetworks/subnets/things@2021-05-01' = [for (kk, k) in range(0, 24): {
  parent: subnet[kk % 6]
  name: string(k)
}]
");
            result.Should().GenerateATemplate();

            var template = result.Template;
            using (new AssertionScope())
            {
                template.Should().HaveValueAtPath("$.resources[0].copy.name", "vnet");
                template.Should().HaveValueAtPath("$.resources[0].name", "[string(copyIndex())]");
                template.Should().NotHaveValueAtPath("$.resources[0].dependsOn");

                template.Should().HaveValueAtPath("$.resources[1].copy.name", "subnet");
                template.Should().HaveValueAtPath("$.resources[1].name", "[format('{0}/{1}', string(mod(copyIndex(), 2)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[1].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks', string(mod(copyIndex(), 2)))]"));

                template.Should().HaveValueAtPath("$.resources[2].copy.name", "thing");
                template.Should().HaveValueAtPath("$.resources[2].name", "[format('{0}/{1}/{2}', string(mod(mod(range(0, 24)[copyIndex()], 6), 2)), string(mod(range(0, 24)[copyIndex()], 6)), string(copyIndex()))]");
                template.Should().HaveValueAtPath("$.resources[2].dependsOn", new JArray("[resourceId('Microsoft.Network/virtualNetworks/subnets', string(mod(mod(range(0, 24)[copyIndex()], 6), 2)), string(mod(range(0, 24)[copyIndex()], 6)))]"));
            }
        }
    }
}

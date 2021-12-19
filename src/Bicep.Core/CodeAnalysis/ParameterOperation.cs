// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;

namespace Bicep.Core.CodeAnalysis
{
    public record ParameterOperation(
        string Name,
        ImmutableArray<ObjectPropertyOperation> AdditionalProperties) : Operation
    {
        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitParameterOperation(this);
    }
}

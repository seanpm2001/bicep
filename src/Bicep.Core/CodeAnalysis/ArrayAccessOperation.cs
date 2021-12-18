// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public record ArrayAccessOperation(
        Operation Base,
        Operation Access) : Operation
    {
        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitArrayAccessOperation(this);
    }
}

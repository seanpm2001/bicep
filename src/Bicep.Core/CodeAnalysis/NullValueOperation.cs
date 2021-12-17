// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public class NullValueOperation : Operation
    {
        public NullValueOperation()
        {
        }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitNullValueOperation(this);
    }
}

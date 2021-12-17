// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public class ForLoopOperation : Operation
    {
        public ForLoopOperation(Operation expression, Operation body)
        {
            this.Expression = expression;
            this.Body = body;
        }

        public Operation Expression { get; }

        public Operation Body { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitForLoopOperation(this);
    }
}

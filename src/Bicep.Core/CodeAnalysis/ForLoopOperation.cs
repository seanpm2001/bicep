// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Bicep.Core.Syntax;

namespace Bicep.Core.CodeAnalysis
{
    public class ForLoopOperation : Operation
    {
        public ForLoopOperation(Operation expression, SyntaxBase itemVariable, SyntaxBase? indexVariable, Operation body)
        {
            this.Expression = expression;
            this.ItemVariable = itemVariable;
            this.IndexVariable = indexVariable;
            this.Body = body;
        }

        public Operation Expression { get; }

        public SyntaxBase ItemVariable { get; }

        public SyntaxBase? IndexVariable { get; }

        public Operation Body { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitForLoopOperation(this);
    }
}

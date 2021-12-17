// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;

namespace Bicep.Core.CodeAnalysis
{
    public class ArrayOperation : Operation
    {
        public ArrayOperation(ImmutableArray<Operation> items)
        {
            Items = items;
        }

        public ImmutableArray<Operation> Items { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitArrayOperation(this);
    }
}

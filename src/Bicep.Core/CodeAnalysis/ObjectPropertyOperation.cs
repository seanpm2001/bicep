// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public class ObjectPropertyOperation : Operation
    {
        public ObjectPropertyOperation(Operation key, Operation value)
        {
            Key = key;
            Value = value;
        }

        public Operation Key { get; }

        public Operation Value { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitObjectPropertyOperation(this);
    }
}

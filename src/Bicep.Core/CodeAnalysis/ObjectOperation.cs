// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Immutable;

namespace Bicep.Core.CodeAnalysis
{
    public class ObjectOperation : Operation
    {
        public ObjectOperation(ImmutableArray<ObjectPropertyOperation> properties)
        {
            Properties = properties;
        }

        public ImmutableArray<ObjectPropertyOperation> Properties { get; }

        public override void Accept(IOperationVisitor visitor)
            => visitor.VisitObjectOperation(this);
    }
}

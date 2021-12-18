// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public abstract record Operation()
    {
        public abstract void Accept(IOperationVisitor visitor);
    }
}

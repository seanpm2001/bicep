// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bicep.Core.CodeAnalysis
{
    public interface IOperationVisitor
    {
        void VisitConstantValueOperation(ConstantValueOperation operation);

        void VisitPropertyAccessOperation(PropertyAccessOperation operation);

        void VisitArrayAccessOperation(ArrayAccessOperation operation);

        void VisitResourceIdOperation(ResourceIdOperation operation);

        void VisitResourceNameOperation(ResourceNameOperation operation);

        void VisitResourceTypeOperation(ResourceTypeOperation operation);

        void VisitResourceApiVersionOperation(ResourceApiVersionOperation operation);

        void VisitResourceReferenceOperation(ResourceReferenceOperation operation);

        void VisitSymbolicResourceReferenceOperation(SymbolicResourceReferenceOperation operation);

        void VisitResourceInfoOperation(ResourceInfoOperation operation);

        void VisitModuleNameOperation(ModuleNameOperation operation);

        void VisitModuleOutputOperation(ModuleOutputOperation operation);

        void VisitVariableAccessOperation(VariableAccessOperation operation);

        void VisitParameterAccessOperation(ParameterAccessOperation operation);

        void VisitModuleReferenceOperation(ModuleReferenceOperation operation);

        void VisitFunctionCallOperation(FunctionCallOperation operation);

        void VisitArrayOperation(ArrayOperation operation);

        void VisitForLoopOperation(ForLoopOperation operation);

        void VisitGetKeyVaultSecretOperation(GetKeyVaultSecretOperation operation);

        void VisitNullValueOperation(NullValueOperation operation);

        void VisitObjectOperation(ObjectOperation operation);

        void VisitObjectPropertyOperation(ObjectPropertyOperation operation);
    }
}

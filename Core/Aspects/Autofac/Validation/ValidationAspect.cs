using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new Exception(AspectMessages.WrongValidationType);
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation, out bool willContinue)
        {
            willContinue = true;
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t.GetType().Equals(entityType));
            foreach (var entity in entities)
            {
                FluentValidationTool.Validate(validator, entity, out var errors);

                if (errors != null)
                {
                    willContinue = false;
                    Type returnType = invocation.Method.ReturnType;
                    Type errorType = typeof(ErrorResult);
                    var isResultData = returnType.IsGenericType && returnType.GetGenericTypeDefinition().Equals(typeof(IDataResult<>));
                    if (isResultData)
                    {
                        errorType = typeof(ErrorDataResult<>);
                        Type constructedErrorType = errorType.MakeGenericType(returnType.GetGenericArguments()[0]);
                        invocation.ReturnValue = Activator.CreateInstance(constructedErrorType, errors.FirstOrDefault().ErrorMessage);
                    }
                    else
                    {
                        invocation.ReturnValue = (ErrorResult)Activator.CreateInstance(errorType, errors.FirstOrDefault().ErrorMessage);
                    }
                    break;
                }
            }
        }
    }
}

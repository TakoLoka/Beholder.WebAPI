using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Interceptors
{
    public abstract class MethodInterception : MethodInterceptorBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation, out bool willContinue)
        {
            willContinue = true;
        }
        protected virtual void OnAfter(IInvocation invocation)
        {
        }
        protected virtual void OnException(IInvocation invocation)
        {
        }
        protected virtual void OnSuccess(IInvocation invocation)
        {
        }
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation, out bool willContinue);
            if (willContinue)
            {
                try
                {
                    invocation.Proceed();
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    OnException(invocation);
                    throw;
                }
                finally
                {
                    if (isSuccess)
                        OnSuccess(invocation);
                }

                OnAfter(invocation);
            }
        }
    }
}

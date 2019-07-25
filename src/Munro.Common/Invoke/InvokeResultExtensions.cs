using System;

namespace Munro.Common.Invoke
{
    public static class InvokeResultExtensions
    {
        public static InvokeResult<T> Return<T>(this T val)
            where T : class
        {
            if (val == null)
            {
                return InvokeResult<T>.Fail(ResultCode.ObjectMissing);
            }
            return InvokeResult<T>.Ok(val);
        }

        public static InvokeResult<TOut> Map<TIn, TOut>(this InvokeResult<TIn> invokeResult, Func<TIn, InvokeResult<TOut>> func)
        {
            if (invokeResult.IsSuccess)
            {
                return func(invokeResult.Result);
            }
            return InvokeResult<TOut>.Fail(invokeResult.Code, invokeResult.ErrorMessage);
        }
    }
}
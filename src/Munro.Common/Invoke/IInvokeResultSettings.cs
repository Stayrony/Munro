using System;

namespace Munro.Common.Invoke
{
    public interface IInvokeResultSettings : ICloneable
    {
        OperationLogSettings ValidationLogSettings  { get; }
        OperationLogSettings LogRequestLogSettings  { get; }
        InvokeFunctionLogSettings InvokeFunctionLogSettings { get; }
    }
}
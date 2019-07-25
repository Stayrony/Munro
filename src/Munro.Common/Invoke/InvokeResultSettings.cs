using Microsoft.Extensions.Logging;

namespace Munro.Common.Invoke
{
    public class InvokeResultSettings : IInvokeResultSettings
    {
        public InvokeResultSettings()
        {
            ValidationLogSettings = new OperationLogSettings
            {
                IsLogging = true,
                LogLevel  = LogLevel.Debug
            };
            
            LogRequestLogSettings = new OperationLogSettings
            {
                IsLogging = true,
                LogLevel  = LogLevel.Debug
            };
            
            InvokeFunctionLogSettings = new InvokeFunctionLogSettings
            {
                LogInvokeResult = LogInvokeResult.IsFail,
                LogLevel  = LogLevel.Debug
            };
        }                
        
        public OperationLogSettings ValidationLogSettings  { get; set; }
        public OperationLogSettings LogRequestLogSettings  { get; set; }
        public InvokeFunctionLogSettings InvokeFunctionLogSettings { get; set; }
        
        public object Clone() => MemberwiseClone();
    }
}
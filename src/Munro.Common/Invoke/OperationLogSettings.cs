﻿using Microsoft.Extensions.Logging;

namespace Munro.Common.Invoke
{
    public class OperationLogSettings
    {
        public bool     IsLogging { get; set; }
        public LogLevel LogLevel  { get; set; }
    }
}
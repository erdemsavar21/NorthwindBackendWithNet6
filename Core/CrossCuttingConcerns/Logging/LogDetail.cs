﻿

namespace Core.CrossCuttingConcerns.Logging
{
    public class LogDetail
    {
        public LogDetail()
        {
        }

        public string MethodName { get; set; }
        public List<LogParameter> LogParameters { get; set; }
    }
}

using System;

namespace FinanceManagement.Core.Common.Constants
{
    public static class HangfireConstants
    {
        private static string selfQueueName = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{Environment.MachineName}".ToLower();

        public const string Self = "self";
        public const string Worker = "worker";

        public static string SelfQueueName => selfQueueName;
    }
}

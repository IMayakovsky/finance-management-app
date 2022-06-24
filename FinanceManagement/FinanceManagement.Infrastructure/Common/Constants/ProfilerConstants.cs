using System;

namespace FinanceManagement.Infrastructure.Common.Constants
{
    public static class ProfilerConstants
    {
        public const string ProfilerRoute = "/profiler";

        public static string GetUrlToProfilationDetail(Guid profilationId)
        {
            return $"{ProfilerRoute}/results?id={profilationId}";
        }
    }
}

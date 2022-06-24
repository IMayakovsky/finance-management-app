namespace FinanceManagement.Infrastructure.Helpers
{
    public static class RequestLogHelper
    {
        public static bool ShouldBeLoggedOrProfiled(string requestUrl)
        {
            if (requestUrl == null) return true;

            if (requestUrl.Contains("/negotiate")) // service hub connection requests
            {
                return false;
            }

            return true;
        }
    }
}

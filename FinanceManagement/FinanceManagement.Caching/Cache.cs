using System;

namespace FinanceManagement.Caching
{
    public static class Cache
    {
        public static IMemoryCache Current { get; private set; }

        public static void Initialize(IMemoryCache cache)
        {
            cache.Enabled = true;
            Current = cache;
        }

        public static string GetKey(Type type, params object[] parameters)
            => GetKey(type.FullName, parameters);

        public static string GetKey(string typeIdentificator, params object[] parameters)
        {
            bool hasParam = parameters.Length > 0;

            return $"{typeIdentificator}" + (hasParam ? $"({string.Join(",", parameters)})" : "");
        }
    }
}

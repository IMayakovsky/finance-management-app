namespace FinanceManagement.Caching
{
    public class CacheValue<T>
    {
        public static CacheValue<T> Null => new CacheValue<T>(default(T), true);
        public static CacheValue<T> NoValue => new CacheValue<T>(default(T), false);

        public bool HasValue { get; }
        public bool IsNull { get; }
        public T Value { get; }

        public CacheValue(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
            IsNull = value == null;
        }
    }
}

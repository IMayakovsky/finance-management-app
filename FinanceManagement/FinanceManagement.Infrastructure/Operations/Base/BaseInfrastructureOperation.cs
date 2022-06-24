using FinanceManagement.Caching;
using FinanceManagement.Caching.Dependencies;
using FinanceManagement.Core.Exceptions;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FinanceManagement.Infrastructure.Operations.Base
{
    public abstract class BaseInfrastructureOperation
    {
        private const string CODE_DESCRIPTION_MEMORYCACHED = "Memory Cached code";
        public const string CODE_DESCRIPTION_PROFILE = "Profiled code";

        #region Profiler

        protected virtual TResult ExecuteAndProfile<TResult>(Func<TResult> function, string codeBlockDescription)
        {
            using (MiniProfiler.Current.Step(codeBlockDescription))
            {
                try
                {
                    return function.Invoke();
                }
                catch (Exception exception)
                {
                    throw new BaseException(CODE_DESCRIPTION_PROFILE + ": " + codeBlockDescription, exception);
                }
            }
        }

        protected virtual TResult ExecuteAndProfile<TResult>(Func<TResult> function, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerMember = null,
            [CallerFilePath] string sourceFilePath = "") => ExecuteAndProfile(function, $"Code at {Path.GetFileNameWithoutExtension(sourceFilePath)}.{callerMember}(..):{lineNumber} (which returns {function})");

        #endregion

        #region Cache

        protected virtual async Task<TResult> ExecuteMemoryCachedAsync<TResult>(
            Func<Task<TResult>> function,
            string cacheKey,
            params CacheDependency[] dependencies)
            => await ExecuteMemoryCachedAsync(function, cacheKey, false, false, dependencies);

        protected virtual async Task<TResult> ExecuteMemoryCachedAsync<TResult>(
            Func<Task<TResult>> function,
            string cacheKey,
            bool ignoreCachedValue,
            params CacheDependency[] dependencies) 
            => await ExecuteMemoryCachedAsync(function, cacheKey, ignoreCachedValue, false, dependencies);

        protected virtual async Task<TResult> ExecuteMemoryCachedAsync<TResult>(
            Func<Task<TResult>> function,
            string cacheKey,
            bool ignoreCachedValue,
            bool cacheNullValue,
            params CacheDependency[] dependencies)
        {
            if (!Cache.Current.Enabled)
            {
                return await function();
            }

            var cacheProvider = Cache.Current.Provider;
            var cached = cacheProvider.Get<TResult>(cacheKey);

            if (!cached.HasValue || ignoreCachedValue)
            {
                try
                {
                    var result = await function.Invoke();

                    if (result != null || cacheNullValue)
                    {
                        Cache.Current.Dependencies.AddDependencies(cacheKey, dependencies);

                        cacheProvider.Set(cacheKey, result);
                    }

                    return result;
                }
                catch (Exception exception)
                {
                    throw new BaseException(CODE_DESCRIPTION_MEMORYCACHED + ": " + cacheKey, exception);
                }
            }

            return cached.Value;
        }

        protected virtual async Task<List<TResult>> ExecuteMemoryCachedMultipleAsync<TKey, TResult>(
           List<TKey> keys,
           Func<TKey, string> keyToCacheKey,
           Func<TResult, TKey> resultToIdFunc,
           Func<List<TKey>, Task<List<TResult>>> fallbackLoad,
           Func<TResult, CacheDependency[]> resultToDependency,
           bool ignoreCachedValue = false)
        {
            string profilerStepName = keys.Count > 0 ? $"{keyToCacheKey(keys[0])}(+{keys.Count - 1} more keys)" : "ExecuteMemoryCachedMultipleAsync(0 keys)";

            using (MiniProfiler.Current.Step(profilerStepName))
            {
                if (!Cache.Current.Enabled)
                {
                    return await fallbackLoad(keys);
                }

                var cacheProvider = Cache.Current.Provider;

                var retValue = new List<TResult>();
                List<TKey> toLoad = null;

                foreach (var key in keys)
                {
                    var cached = cacheProvider.Get<TResult>(keyToCacheKey(key));
                    if (cached.HasValue && !ignoreCachedValue)
                    {
                        retValue.Add(cached.Value);
                    }
                    else
                    {
                        if (toLoad == null)
                        {
                            toLoad = new List<TKey>();
                        }

                        toLoad.Add(key);
                    }
                }

                if (toLoad != null && toLoad.Any())
                {
                    try
                    {
                        var loaded = await fallbackLoad(toLoad);

                        foreach (TResult loadedItem in loaded)
                        {
                            if (loadedItem == null) continue;

                            TKey key = resultToIdFunc(loadedItem);
                            var cacheKey = keyToCacheKey(key);

                            Cache.Current.Dependencies.AddDependencies(cacheKey, resultToDependency(loadedItem));

                            cacheProvider.Set(cacheKey, loadedItem);

                            retValue.Add(loadedItem);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new BaseException(CODE_DESCRIPTION_MEMORYCACHED + " " + nameof(ExecuteMemoryCachedMultipleAsync), e);
                    }
                }

                return retValue;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FinanceManagement.Caching.Dependencies
{
    public interface ICacheDependencyManager
    {
        void AddDependencies(string key, params CacheDependency[] dependencies);

        void RemoveDependencies(string key, params CacheDependency[] dependencies);

        void InvalidateByDependencies(CacheDependencyType dependencyType, params int[] ids);
    }

    public class CacheDependencyManager : ICacheDependencyManager
    {
        private ICacheProvider provider;

        private ConcurrentDictionary<CacheDependencyType, ConcurrentDictionary<int, HashSet<string>>> dependencies;

        public CacheDependencyManager(ICacheProvider provider)
        {
            this.provider = provider;

            dependencies = new ConcurrentDictionary<CacheDependencyType, ConcurrentDictionary<int, HashSet<string>>>();
        }

        public void AddDependencies(string key, params CacheDependency[] dependencies)
        {
            foreach (CacheDependency dependency in dependencies)
            {
                var dictionary = this.dependencies.GetOrAdd(dependency.Type,
                    (type) => new ConcurrentDictionary<int, HashSet<string>>());

                AddToDictionary(dictionary, dependency.Id, key);
            }
        }

        public void RemoveDependencies(string key, params CacheDependency[] dependencies)
        {
            foreach (CacheDependency dependency in dependencies)
            {
                var dictionary = this.dependencies.GetOrAdd(dependency.Type,
                    (type) => new ConcurrentDictionary<int, HashSet<string>>());
            
                RemoveFromDictionary(dictionary, dependency.Id, key);
            }
        }

        public void InvalidateByDependencies(CacheDependencyType dependencyType, params int[] ids)
        {
            InvalidateByDependencies(provider.RemoveAll, dependencyType, ids);
        }

        public void InvalidateByDependencies(Action<IEnumerable<string>> providerRemoveFunction, CacheDependencyType dependencyType, params int[] ids)
        {
            var dictionary = dependencies.GetOrAdd(dependencyType, (type) => new ConcurrentDictionary<int, HashSet<string>>());

            if (ids.Length == 0)
            {
                ids = dictionary.Keys.ToArray();
            }

            foreach (int id in ids)
            {
                if (dictionary.TryRemove(id, out var set))
                {
                    lock (set)
                    {
                        providerRemoveFunction(set);
                    }
                }
            }
        }

        private static void AddToDictionary(ConcurrentDictionary<int, HashSet<string>> dict, int id, string key)
        {
            var set = dict.GetOrAdd(id, (nid) => new HashSet<string>());

            lock (set)
            {
                set.Add(key);
            }
        }

        private static void RemoveFromDictionary(ConcurrentDictionary<int, HashSet<string>> dict, int id, string key)
        {
            var set = dict.GetOrAdd(id, (nid) => new HashSet<string>());

            lock (set)
            {
                set.Remove(key);
            }
        }
    }
}

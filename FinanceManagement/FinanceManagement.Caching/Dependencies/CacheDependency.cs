namespace FinanceManagement.Caching.Dependencies
{
    public struct CacheDependency
    {
        public CacheDependencyType Type { get; }
        public int Id { get; }

        public CacheDependency(CacheDependencyType dependencyType, int dependencyId)
        {
            this.Id = dependencyId;
            this.Type = dependencyType;
        }

        public CacheDependency(CacheDependencyType dependencyType)
        {
            this.Type = dependencyType;
            this.Id = 0;
        }
    }
}

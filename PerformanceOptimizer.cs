public class PerformanceOptimizer
{
    private readonly Dictionary<string,CacheItem> _cache = new Dictionary<string, CacheItem>();
    private readonly object _cacheLock = new object();

    public bool TryGetFromCache<T>(string key, out T value)
    {
        value= default(T);
        lock(_cacheLock)
        {
            if(_cache.TryGetValue(key,out var item) && DateTime.UtcNow< item.Expiry)
            {
                value=(T)item.Data;
                return true;
            }
            if (item!=null)
            {
                _cache.Remove(key);
            }
            return false;
        }
    }
    public void SetCache<T>(string key, T value, TimeSpan expiry)
    {
        lock(_cacheLock)
        {
            _cache[key]= new CacheItem
            {
                Data=value,
                Expiry= DateTime.UtcNow.Add(expiry),
            };
        }
    }


}
  public class CacheItem
    {
        public object Data { get; set; }
        public DateTime Expiry { get; set; }
    }
using Company.Core.Configs;
using Company.Core.Extensions;
using Company.Core.Helpers;
using Company.Core.Ioc;
using Company.Logger;
using System.Diagnostics.CodeAnalysis;

namespace Company.Core.Cache
{
    [ExposedService(types: [typeof(ICacheManager)])]
    public class CacheManager : ICacheManager
    {
        private readonly IConfigManager _configManager;
        private Dictionary<string, string> _cacheNames;
        private readonly object _lock = new();

        public CacheManager(IConfigManager configManager)
        {
            _configManager = configManager;
            Read();
        }

        [MemberNotNull(nameof(_cacheNames))]
        private void Read()
        {
            _cacheNames = _configManager.Read<Dictionary<string, string>>(ConfigKey.CacheConfig) ?? [];
        }

        public bool Get<T>(ValueType key, out T? value)
        {
            value = default;
            string? obj;

            lock (_lock)
            {
                if (!_cacheNames.TryGetValue(key.GetFullName(), out obj))
                    return false;
            }

            try
            {
                value = JsonHelper.Deserialize<T>(obj);
                return true;
            }
            catch (Exception e)
            {
                NLogger.Error(e);
                return false;
            }
        }

        public void Set<T>(ValueType key, T value)
        {
            Assert.NotNull(key);
            Assert.NotNull(value);

            string content = JsonHelper.Serialize(value!, false);
            lock(_lock)
            {
                _cacheNames[key.GetFullName()] = content;
                _configManager.Write(ConfigKey.CacheConfig, _cacheNames);
            }
        }

        public void Delete(ValueType key)
        {
            lock (_lock)
            {
                _cacheNames.Remove(key.GetFullName());
                _configManager.Write(ConfigKey.CacheConfig, _cacheNames);
            }
        }
    }
}

using Company.Core.Helpers;
using Company.Core.Ioc;
using System.IO;

namespace Company.Core.Configs
{
    [ExposedService(Lifetime.Singleton, true, typeof(IConfigManager))]
    public class ConfigManager : IConfigManager
    {
        private const string ConfigDirName = "Configs";
        private const string ConfigFileExt = ".json";

        public readonly string ConfigDirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigDirName);

        private static string GetFullPath(ValueType key)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, ConfigDirName, $"{key.GetType().FullName}.{key}{ConfigFileExt}");
        }

        public T? Read<T>(ValueType key)
        {
            var filePath = GetFullPath(key);
            return JsonHelper.Load<T>(filePath);
        }

        public void Write<T>(ValueType key, T value)
        {
            Assert.NotNull(key);
            Assert.NotNull(value);
            Directory.CreateDirectory(ConfigDirPath);
            var filePath = GetFullPath(key);
            JsonHelper.Save(value!, filePath);
        }
    }
}

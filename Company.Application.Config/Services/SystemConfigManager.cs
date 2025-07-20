using Company.Application.Config.Models;
using Company.Core.Configs;
using Company.Core.Ioc;

namespace Company.Application.Config.Services
{
    /// <summary>
    /// 系统配置管理者，作用是整个系统所有配置的读写工作
    /// </summary>
    [ExposedService]
    public class SystemConfigManager
    {
        private IConfigManager ConfigManager { get; }

        public SystemConfigModel Config { get; private set; }

        public SystemConfigManager(IConfigManager configManager)
        {
            ConfigManager = configManager;

            // 加载配置
            Config = LoadConfig();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        public void Load()
        {
            Config = LoadConfig();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        private SystemConfigModel LoadConfig()
        {
            return ConfigManager.Read<SystemConfigModel>(ConfigKey.SystemConfig) ?? new SystemConfigModel();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void Save()
        {
            ConfigManager.Write(ConfigKey.SystemConfig, Config);
        }
    }
}

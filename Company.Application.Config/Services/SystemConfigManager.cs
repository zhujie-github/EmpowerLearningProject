using Company.Application.Config.Models;
using Company.Core.Configs;
using Company.Core.Ioc;

namespace Company.Application.Config.Services
{
    /// <summary>
    /// 系统配置管理者，作用是整个系统所有配置的读写工作
    /// </summary>
    [ExposedService(Lifetime.Singleton, true)]
    public class SystemConfigManager
    {
        private IConfigManager _configManager;

        public SystemConfigModel Config { get; set; }

        public SystemConfigManager(IConfigManager configManager)
        {
            _configManager = configManager;

            Load();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void Load()
        {
            Config = _configManager.Read<SystemConfigModel>(ConfigKey.SystemConfig) ?? new SystemConfigModel();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void Save()
        {
            _configManager.Write(ConfigKey.SystemConfig, Config);
        }
    }
}

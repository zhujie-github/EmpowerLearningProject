namespace Company.Core.Configs
{
    /// <summary>
    /// 配置管理器接口
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 读取本地配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T? Read<T>(ValueType key);

        /// <summary>
        /// 保存本地配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Write<T>(ValueType key, T value);
    }
}

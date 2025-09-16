namespace Company.Core.Cache
{
    /// <summary>
    /// 缓存类型
    /// </summary>
    public enum CacheKey
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        User,

        /// <summary>
        /// 是否自动登录
        /// </summary>
        IsAutoLogin,

        /// <summary>
        /// 是否记住用户名、密码
        /// </summary>
        IsRemember,

        /// <summary>
        /// 缓存数据
        /// </summary>
        AppData,

        /// <summary>
        /// 过滤器算法缓存数据
        /// </summary>
        Filters,
    }
}

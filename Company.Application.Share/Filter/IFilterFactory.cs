namespace Company.Application.Share.Filter
{
    /// <summary>
    /// 创建所有图像算法模型的工厂
    /// </summary>
    public interface IFilterFactory
    {
        /// <summary>
        /// 实例化所有滤波算法
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns></returns>
        List<IFilterModel> CreateFilterModels();
    }
}

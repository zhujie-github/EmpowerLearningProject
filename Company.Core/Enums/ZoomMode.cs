using System.ComponentModel;

namespace Company.Core.Enums
{
    /// <summary>
    /// 图像缩放模式
    /// </summary>
    public enum ZoomMode
    {
        [Description("自适应")]
        Uniform,

        [Description("原始大小")]
        Original,

        [Description("放大为200%")]
        Percent200,

        [Description("放大为300%")]
        Percent300,

        [Description("放大为400%")]
        Percent400,

        [Description("放大为500%")]
        Percent500,

        [Description("放大为600%")]
        Percent600,

        [Description("放大为700%")]
        Percent700,

        [Description("放大为800%")]
        Percent800,
    }
}

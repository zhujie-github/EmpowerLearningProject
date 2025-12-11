namespace Company.Application.Share.Draw
{
    /// <summary>
    /// 缩放移动对象的提供者
    /// </summary>
    public interface ITransformProvider
    {
        TransformModel Transform { get; }
        IObservable<TransformModel> TransformObservable { get; }
    }
}
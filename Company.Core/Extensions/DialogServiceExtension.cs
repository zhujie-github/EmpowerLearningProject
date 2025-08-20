namespace Company.Core.Extensions
{
    /// <summary>
    /// 对话框扩展类
    /// </summary>
    public static class DialogServiceExtension
    {
        /// <summary>
        /// 以阻塞的方式弹出非模态对话框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <param name="parameters"></param>
        public static void ShowDialog<T>(this IDialogService dialogService, Action<IDialogResult> callback,
            DialogParameters? parameters = null)
        {
            dialogService.ShowDialog(typeof(T).Name, parameters ?? [], callback);
        }
    }
}

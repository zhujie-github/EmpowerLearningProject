using ReactiveUI;

namespace Company.Core.Ioc
{
    public class DialogViewModelBase : ReactiveObject, IDialogAware
    {
        public string Title { get; set; } = "";

        public DialogCloseListener RequestClose { get; }

        /// <summary>
        /// 判断是否可以关闭对话框
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// 对话框关闭时触发该方法
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnDialogClosed()
        {
            
        }

        /// <summary>
        /// 对话框打开时触发该方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void OnDialogOpened(IDialogParameters parameters)
        {

        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="dialogResult"></param>
        /// <param name="dialogParameters"></param>
        protected virtual void CloseDialog(ButtonResult buttonResult, IDialogParameters? dialogParameters = null)
        {
            var dialogResult = new DialogResult
            {
                Result = buttonResult
            };
            if (dialogParameters != null)
            {
                dialogResult.Parameters = new DialogParameters { { "MyParameter", dialogParameters } };
            }

            RequestClose.Invoke(dialogResult);
        }
    }
}

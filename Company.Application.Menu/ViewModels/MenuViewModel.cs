using Company.Application.Share.Prism;
using Company.Core.Ioc;
using System.Windows.Input;

namespace Company.Application.Menu.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public ICommand SystemConfigCommand { get; }

        public MenuViewModel()
        {
            SystemConfigCommand = new DelegateCommand(SystemConfig);
        }

        private void SystemConfig()
        {
            PrismProvider.DialogService.ShowDialog(ViewNames.ConfigView, Callback);
        }

        private void Callback(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                // 处理确认逻辑
            }
            else if (result.Result == ButtonResult.Cancel)
            {
                // 处理取消逻辑
            }
        }
    }
}

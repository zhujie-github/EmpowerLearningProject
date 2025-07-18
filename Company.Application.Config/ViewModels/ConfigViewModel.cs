using Company.Application.Config.Services;
using Company.Application.Share.Events;
using Company.Core.Ioc;
using ReactiveUI;
using System.Windows.Input;

namespace Company.Application.Config.ViewModels
{
    public class ConfigViewModel : DialogViewModelBase
    {
        private SystemConfigManager SystemConfigManager { get; }

        public SystemConfigProvider SystemConfigProvider { get; }

        public ICommand OkCommand { get; }

        public ConfigViewModel(SystemConfigManager systemConfigManager, SystemConfigProvider systemConfigProvider)
        {
            SystemConfigManager = systemConfigManager;
            SystemConfigProvider = systemConfigProvider;
            OkCommand = ReactiveCommand.Create(Submit);
            Title = "参数设置";
        }

        private void Submit()
        {
            SystemConfigManager.Save();

            CloseDialog(ButtonResult.OK);

            PrismProvider.EventAggregator?.GetEvent<SoftwareConfigChangedEvent>().Publish();
        }
    }
}

using Company.Application.Initialize.Services;
using Company.Application.Share.Prism;
using Company.Core.Events;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Input;

namespace Company.Application.Initialize.ViewModels
{
    public class InitializeViewModel : ReactiveObject
    {
        private readonly HardwareLifetimeManager _hardwareLifetimeManager;

        [Reactive]
        public string? Message { get; set; }

        public ICommand LoadedCommand { get; set; }

        public InitializeViewModel(HardwareLifetimeManager hardwareLifetimeManager)
        {
            _hardwareLifetimeManager = hardwareLifetimeManager;

            LoadedCommand = ReactiveCommand.Create(Loaded);
            PrismProvider.EventAggregator.GetEvent<CloseAllHardwareEvent>().Subscribe(_hardwareLifetimeManager.Close);
        }

        private async Task Loaded()
        {
            if (!_hardwareLifetimeManager.IsInitialized)
            {
                Message = "正在初始化硬件，请稍候...";
                var result = await _hardwareLifetimeManager.InitAsync();
                Message = $"硬件初始化{(result.Item1 ? "成功" : "失败")}: {result.Item2}".TrimEnd(' ', ':');
                if (!result.Item1)
                {
                    return;
                }
            }

            //加载主界面
            await Task.Delay(1000).ContinueWith(p => {
                PrismProvider.Dispatcher?.Invoke(() =>
                {
                    PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, ViewNames.MainView);
                });
            });
        }
    }
}

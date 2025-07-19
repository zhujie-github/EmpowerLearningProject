using Company.Application.Share.Configs;
using Company.Application.Share.Prism;
using Company.Core.Events;
using Company.Core.Ioc;
using Company.Core.Models;
using System.Windows;
using System.Windows.Input;

namespace Shell.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private CurrentUser? CurrentUser { get; set; }

        public IModuleManager ModuleManager { get; }

        public IRegionManager RegionManager { get; }

        public ICommand LoadedCommand { get; }

        public MainWindowViewModel(IModuleManager moduleManager, IRegionManager regionManager)
        {
            ModuleManager = moduleManager;
            RegionManager = regionManager;
            LoadedCommand = new DelegateCommand(OnLoaded);
        }

        private void OnLoaded()
        {
            var mainRegion = PrismProvider.RegionManager?.Regions[RegionNames.MainRegion];
            if (mainRegion != null)
                mainRegion.NavigationService.Navigated += NavigationService_Navigated;

            // 确保登录模块已加载
            //PrismProvider.ModuleManager?.LoadModule(ModuleNames.ApplicationLoginModule);

            // 导航到登录视图
            PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, ViewNames.LoginView);

            // 订阅事件
            PrismProvider.EventAggregator?.GetEvent<LoginSucceededEvent>().Subscribe(OnLoginSucceeded);
            PrismProvider.EventAggregator?.GetEvent<LoginCancelledEvent>().Subscribe(OnLoginCancelled);
            PrismProvider.EventAggregator?.GetEvent<ConfigChangedEvent>().Subscribe(OnConfigChanged);
        }

        public bool IsHardwareInitialized { get; private set; } = false;

        private void OnLoginSucceeded(CurrentUser user)
        {
            CurrentUser = user;

            if (IsHardwareInitialized)
            {
                // 登录成功后，导航到主界面或其他视图
                //PrismProvider.ModuleManager?.LoadModule(ModuleNames.ApplicationMainModule);
                PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, ViewNames.MainView);
            }
            else
            {
                PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, ViewNames.InitializeView);
            }

            // 可以在这里处理登录成功后的逻辑，比如显示欢迎信息等
            Application.Current.MainWindow.WindowState = WindowState.Maximized; // 最大化窗口
            ChangeMainWindowTitle(); // 更新窗口标题
        }

        private Window MainWindow { get; } = Application.Current.MainWindow;

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            switch(e.Uri.OriginalString)
            {
                case ViewNames.LoginView:
                    // 登录视图被导航到时，可以执行一些特定的操作
                    MainWindow.ResizeMode = ResizeMode.NoResize; // 禁止调整大小
                    MainWindow.SizeToContent = SizeToContent.WidthAndHeight; // 根据内容自动调整大小
                    MainWindow.WindowState = WindowState.Normal; // 确保窗口处于正常状态
                    MainWindow.WindowStyle = WindowStyle.None; // 设置窗口样式为无边框
                    break;

                case ViewNames.MainView:
                    // 主视图被导航到时，可以执行一些特定的操作
                    MainWindow.ResizeMode = ResizeMode.CanResize; // 允许调整大小
                    MainWindow.SizeToContent = SizeToContent.Manual; // 手动设置大小
                    MainWindow.WindowState = WindowState.Maximized; // 最大化窗口
                    MainWindow.WindowStyle = WindowStyle.SingleBorderWindow; // 设置窗口样式为单边框窗口
                    break;
                default:
                    break;
            }
        }

        private void OnLoginCancelled()
        {
            // 登录取消后，可以执行一些清理操作或关闭应用程序
            Application.Current.Shutdown(); // 关闭应用程序
        }

        /// <summary>
        /// 更新主窗口标题
        /// </summary>
        private void ChangeMainWindowTitle()
        {
            var softwareName = PrismProvider.Container?.Resolve<ISystemConfigProvider>()?.SoftwareConfig?.Name ?? "";
            Application.Current.MainWindow.Title = $"{softwareName} - {CurrentUser?.UserName}";
        }

        /// <summary>
        /// 配置修改时触发该方法
        /// </summary>
        private void OnConfigChanged()
        {
            ChangeMainWindowTitle();
        }
    }
}

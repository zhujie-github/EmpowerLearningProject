using Company.Application.Login;
using Company.Application.Login.Views;
using Company.Application.Main.Views;
using Company.Application.Share.Events;
using Company.Application.Share.Models;
using Company.Application.Share.Prism;
using Company.Core.Ioc;
using System.Windows;
using System.Windows.Input;

namespace Shell.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        public string Title { get; } = "学习程序";

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
            PrismProvider.ModuleManager?.LoadModule(nameof(ApplicationLoginModule));

            // 导航到登录视图
            PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, nameof(LoginView));

            // 订阅登录成功、登录取消事件
            PrismProvider.EventAggregator?.GetEvent<LoginSucceededEvent>().Subscribe(OnLoginSucceeded);
            PrismProvider.EventAggregator?.GetEvent<LoginCancelledEvent>().Subscribe(OnLoginCancelled);
        }

        private void OnLoginSucceeded(CurrentUser user)
        {
            // 登录成功后，导航到主界面或其他视图
            PrismProvider.RegionManager?.RequestNavigate(RegionNames.MainRegion, nameof(MainView));

            // 可以在这里处理登录成功后的逻辑，比如显示欢迎信息等
            Application.Current.MainWindow.WindowState = WindowState.Maximized; // 最大化窗口
            Application.Current.MainWindow.Title = $"{Title} - {user.UserName}"; // 更新窗口标题
            MessageBox.Show($"欢迎回来，{user.UserName}！", "登录成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private Window MainWindow { get; } = Application.Current.MainWindow;

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            switch(e.Uri.OriginalString)
            {
                case nameof(LoginView):
                    // 登录视图被导航到时，可以执行一些特定的操作
                    MainWindow.ResizeMode = ResizeMode.NoResize; // 禁止调整大小
                    MainWindow.SizeToContent = SizeToContent.WidthAndHeight; // 根据内容自动调整大小
                    MainWindow.WindowState = WindowState.Normal; // 确保窗口处于正常状态
                    MainWindow.WindowStyle = WindowStyle.None; // 设置窗口样式为无边框
                    break;

                case nameof(MainView):
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
    }
}

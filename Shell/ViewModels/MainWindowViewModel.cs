using Company.Application.Login;
using Company.Application.Login.Views;
using Company.Application.Main.Views;
using Company.Application.Share.Events;
using Company.Application.Share.Models;
using Company.Application.Share.Prism;
using Company.Core.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void OnLoginCancelled()
        {
            // 登录取消后，可以执行一些清理操作或关闭应用程序
            Application.Current.Shutdown(); // 关闭应用程序
        }
    }
}

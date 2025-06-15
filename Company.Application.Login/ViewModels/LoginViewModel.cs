using Company.Application.Share.Events;
using Company.Application.Share.Models;
using Company.Core.Ioc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Company.Application.Login.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private CurrentUser CurrentUser { get; set; } = new CurrentUser();

        private string? _login;
        public string? Login
        {
            get => _login;
            set
            {
                SetProperty(ref _login, value);
                LogInCommand.RaiseCanExecuteChanged(); // 更新登录按钮的可用状态
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                LogInCommand.RaiseCanExecuteChanged(); // 更新登录按钮的可用状态
            }
        }

        public DelegateCommand LogInCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public LoginViewModel()
        {
            LogInCommand = new DelegateCommand(OnLogIn, CanLogIn);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private bool CanLogIn()
        {
            return string.Equals(Login, "admin", StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Password, "123", StringComparison.Ordinal);
        }

        private void OnLogIn()
        {
            if (!CanLogIn())
            {
                MessageBox.Show("用户名或密码错误，请重试！", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentUser.UserName = Login;
            CurrentUser.Password = Password;
            CurrentUser.LastLoginTime = DateTime.Now;
            PrismProvider.EventAggregator?.GetEvent<LoginSucceededEvent>().Publish(CurrentUser);
        }

        private void OnCancel()
        {
            PrismProvider.EventAggregator?.GetEvent<LoginCancelledEvent>().Publish();
        }
    }
}
using Company.Core.Cache;
using Company.Core.Dialogs;
using Company.Core.Events;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI.Fody.Helpers;

namespace Company.Application.Login.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private CurrentUser CurrentUser { get; set; } = new CurrentUser();
        private ICacheManager CacheManager { get; }

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

        [Reactive]
        public bool IsRemember { get; set; }

        [Reactive]
        public bool IsAutoLogin { get; set; }

        public DelegateCommand LogInCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public LoginViewModel(ICacheManager cacheManager)
        {
            CacheManager = cacheManager;
            LogInCommand = new DelegateCommand(OnLogIn, CanLogIn);
            CancelCommand = new DelegateCommand(OnCancel);
            LoadUserCache();

            if (IsAutoLogin)
            {
                Task.Delay(1500).ContinueWith(t => {
                    LogInCommand.Execute();
                });
            }
        }

        private bool CanLogIn()
        {
            return true;
        }

        private void OnLogIn()
        {
            if (!(string.Equals(Login, "admin", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Password, "123", StringComparison.Ordinal)))
            {
                PopupBox.Show("用户名或密码错误");
                return;
            }

            CurrentUser.UserName = Login;
            CurrentUser.Password = Password;
            CurrentUser.LastLoginTime = DateTime.Now;

            if (IsRemember)
            {
                CacheManager.Set(CacheKey.User, CurrentUser);
            }
            else
            {
                CacheManager.Delete(CacheKey.User);
            }
            CacheManager.Set(CacheKey.IsRemember, IsRemember);
            CacheManager.Set(CacheKey.IsAutoLogin, IsAutoLogin);

            PrismProvider.EventAggregator?.GetEvent<LoginSucceededEvent>().Publish(CurrentUser);
        }

        private void OnCancel()
        {
            PrismProvider.EventAggregator?.GetEvent<LoginCancelledEvent>().Publish();
        }

        /// <summary>
        /// 获取用户缓存
        /// </summary>
        private void LoadUserCache()
        {
            if (CacheManager.Get(CacheKey.User, out CurrentUser? user) && user != null)
            {
                CurrentUser = user;
                Login = user.UserName;
                Password = user.Password;
            }
            if (CacheManager.Get(CacheKey.IsRemember, out bool isRemember))
            {
                IsRemember = isRemember;
            }
            if (CacheManager.Get(CacheKey.IsAutoLogin, out bool isAutoLogin))
            {
                IsAutoLogin = isAutoLogin;
            }
        }
    }
}
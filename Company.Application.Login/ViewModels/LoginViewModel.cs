using Company.Application.Share.Events;
using Company.Core.Cache;
using Company.Core.Dialogs;
using Company.Core.Ioc;
using Company.Core.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace Company.Application.Login.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private ICacheManager CacheManager { get; }

        [Reactive]
        public CurrentUser CurrentUser { get; set; }

        [Reactive]
        public bool IsRemember { get; set; }

        [Reactive]
        public bool IsAutoLogin { get; set; }

        public DelegateCommand LoadedCommand { get; set; }
        public DelegateCommand LogInCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public LoginViewModel(ICacheManager cacheManager)
        {
            CacheManager = cacheManager;
            LoadedCommand = new DelegateCommand(Loaded);
            LogInCommand = new DelegateCommand(LogIn);
            CancelCommand = new DelegateCommand(Cancel);
            LoadUserCache();
        }

        private void Loaded()
        {
            Task.Delay(1500).ContinueWith(t =>
            {
                if (IsAutoLogin)
                {
                    LogInCommand.Execute();
                }
            });
        }

        private void LogIn()
        {
            if (!(string.Equals(CurrentUser.UserName, "admin", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(CurrentUser.Password, "123", StringComparison.Ordinal)))
            {
                PopupBox.Show("用户名或密码错误");
                return;
            }

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

        private void Cancel()
        {
            PrismProvider.EventAggregator?.GetEvent<LoginCancelledEvent>().Publish();
        }

        /// <summary>
        /// 获取用户缓存
        /// </summary>
        [MemberNotNull(nameof(CurrentUser))]
        private void LoadUserCache()
        {
            CacheManager.Get(CacheKey.User, out CurrentUser? user);
            CurrentUser = user ?? new CurrentUser();

            CacheManager.Get(CacheKey.IsRemember, out bool isRemember);
            IsRemember = isRemember;

            CacheManager.Get(CacheKey.IsAutoLogin, out bool isAutoLogin);
            IsAutoLogin = isAutoLogin;
        }
    }
}
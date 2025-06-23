using Company.Core.Enums;
using Company.Core.Helpers;
using Company.Core.Ioc;
using System.Windows;

namespace Company.Core.Language
{
    [ExposedService(Lifetime.Singleton, true, typeof(ILanguageManager))]
    public class LanguageManager : ILanguageManager
    {
        private ResourceDictionary? _resourceDictionary;
        private string? _uri;

        public LanguageManager()
        {
            SetLanguage(LanguageType.CN);
        }

        public string this[string key]
        {
            get
            {
                if (_resourceDictionary?.Contains(key) ?? false)
                {
                    return _resourceDictionary?[key]?.ToString() ?? string.Empty;
                }
                return this[key];
            }
        }

        public LanguageType Current { get; set; }

        public void SetLanguage(LanguageType languageType)
        {
            Assert.NotNull(languageType);

            if (_uri == null)
            {
                var resourceDictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault();
                var path = resourceDictionary?.Source?.AbsolutePath;
                _uri = path?[..path.LastIndexOf('/')];
            }

            var target = $"{_uri}/{languageType}.xaml";
            _resourceDictionary = (ResourceDictionary)Application.LoadComponent(new Uri(target, UriKind.RelativeOrAbsolute));
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            Application.Current.Resources.MergedDictionaries.Insert(0, _resourceDictionary);

            if (Current != languageType)
            {
                Current = languageType;
            }
        }
    }
}

using Company.Core.Enums;
using Company.Core.Ioc;

namespace Company.Core.Language
{
    public interface ILanguageManager
    {
        string this[string key] { get; }

        LanguageType Current { get; }

        void Set(LanguageType languageType);
    }
}

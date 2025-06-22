namespace Company.Core.Language
{
    public interface ILanguageManager
    {
        string this[string key] { get; }
    }
}

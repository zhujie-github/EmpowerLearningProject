using Company.Core.Helpers;

namespace Company.Core.Extensions
{
    public static class JsonExtension
    {
        public static T? DeepClone<T>(this T obj) where T : class
        {
            var json = JsonHelper.Serialize(obj);
            return JsonHelper.Deserialize<T>(json);
        }
    }
}

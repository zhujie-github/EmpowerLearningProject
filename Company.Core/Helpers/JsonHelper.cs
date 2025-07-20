using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Core.Helpers
{
    /// <summary>
    /// JSON文件的序列化和反序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 通用序列化器选项
        /// </summary>
        public static JsonSerializerOptions GeneralSerializerOptions { get; } = new()
        {
            WriteIndented = true,
            //PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 允许非 ASCII 字符（如中文）
            PropertyNameCaseInsensitive = true,                    // 不区分大小写反序列化
            //DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,      // 字典 Key 转 camelCase
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // 忽略 null 值
        };

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string Serialize(object obj, bool intended = true)
        {
            var option = new JsonSerializerOptions()
            {
                WriteIndented = intended,
                PropertyNamingPolicy = GeneralSerializerOptions.PropertyNamingPolicy,
                Encoder = GeneralSerializerOptions.Encoder,
                PropertyNameCaseInsensitive = GeneralSerializerOptions.PropertyNameCaseInsensitive,
                DictionaryKeyPolicy = GeneralSerializerOptions.DictionaryKeyPolicy,
                DefaultIgnoreCondition = GeneralSerializerOptions.DefaultIgnoreCondition
            };
            option!.WriteIndented = intended;
            return JsonSerializer.Serialize(obj, option);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T? Deserialize<T>(string content)
        {
            return JsonSerializer.Deserialize<T>(content, GeneralSerializerOptions);
        }

        /// <summary>
        /// 加载某个JSON文件并返回指定的类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T? Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return default;
            }

            var content = File.ReadAllText(filePath);
            return Deserialize<T>(content);
        }

        /// <summary>
        /// 将指定的类型实例保存位JSON文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <param name="indented"></param>
        public static void Save(object obj, string filePath, bool intended = true)
        {
            var content = Serialize(obj, intended);
            File.WriteAllText(filePath, content);
        }
    }
}

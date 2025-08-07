using Company.Core.Helpers;
using Company.Core.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Company.Core.Extensions
{
    public static class ComboBoxExtension
    {
        private static string GetEnumDescription(this object obj)
        {
            Assert.NotNull(obj);
            var type = obj.GetType();
            if (!type.IsEnum)
            {
                throw new Exception($"{type.FullName}必须为枚举类型");
            }

            var fieldInfo = type.GetField(obj.ToString()!);
            if (fieldInfo != null)
            {
                var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as
                    DescriptionAttribute;
                return attribute?.Description ?? fieldInfo.Name;
            }

            return string.Empty;
        }

        private static IEnumerable<BindableSourceItem<T>> GetBindableSourceItems<T>(this object obj,
            Func<T, bool>? filter = null) where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception($"{type.FullName}必须为枚举类型");
            }

            var items = type.GetEnumValues().Cast<T>().Select( x => new BindableSourceItem<T>()
            {
                Description = x.GetEnumDescription(),
                Value = x,
            });

            if (filter != null)
            {
                items = items.Where(i => filter(i.Value));
            }

            return items;
        }

        public static void Binding<T>(this ComboBox comboBox, Func<T, bool>? filter = null, bool icon = false)
            where T : struct
        { 
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception($"{type.FullName}必须为枚举类型");
            }

            var items = type.GetBindableSourceItems<T>(filter);
            comboBox.DoBinding(items, icon);
        }

        private static void DoBinding<T>(this ComboBox comboBox, IEnumerable<BindableSourceItem<T>> items,
            bool icon = false) where T : struct
        {
            if (icon)
            {
                var fef= new FrameworkElementFactory(typeof(Image));
                var binding = new System.Windows.Data.Binding
                {
                    Path = new PropertyPath(nameof(BindableSourceItem<T>.Description))
                };
                fef.SetBinding(Image.SourceProperty, binding);
                fef.SetValue(FrameworkElement.HeightProperty, 30.0);
                var template = new DataTemplate() { VisualTree = fef };
                template.Seal();
                comboBox.ItemTemplate = template;

            }
            else
            {
                comboBox.DisplayMemberPath = nameof(BindableSourceItem<T>.Description);
            }

            comboBox.SelectedValuePath = nameof(BindableSourceItem<T>.Value);
            comboBox.ItemsSource = items;
        }
    }
}

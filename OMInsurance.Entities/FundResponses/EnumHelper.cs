using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace OMInsurance.Entities
{
    public static class EnumHelper<T> where T : struct
    {
        public static Dictionary<T, string> GetValues()
        {
            var enumValues = new Dictionary<T, string>();
            var enumType = typeof(T);
            foreach (FieldInfo fi in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                T value = (T)Enum.Parse(enumType, fi.Name, false);
                enumValues.Add(value, EnumHelper<T>.GetDisplayValue(value));
            }
            return enumValues;
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}

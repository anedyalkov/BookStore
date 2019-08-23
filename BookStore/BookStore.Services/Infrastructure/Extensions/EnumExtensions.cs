using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookStore.Services.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum en)
        {
            var attr = GetDisplayAttribute(en);
            return attr != null ? attr.Name : en.ToString();
        }

        public static string GetDescription(this Enum en)
        {
            var attr = GetDisplayAttribute(en);
            return attr != null ? attr.Description : en.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            var field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}

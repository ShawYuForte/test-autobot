using System;
using System.Linq;
using forte.models;

namespace forte.extensions
{
    public static class EnumExtensions
    {
        public static string ToCharCode(this Enum value)
        {
            if (value == null)
            {
                return null;
            }

            var field = value.GetType().GetField(value.ToString());

            var attributes = field.GetCustomAttributes(typeof(CharCodeAttribute), false)
                as CharCodeAttribute[];

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].GetCode();
            }

            return default(string);
        }

        public static Enum ToCodeEnum<T>(this string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                var targetType = typeof(T);
                var targetNullable = targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>);
                return targetNullable ? null : (Enum)Enum.GetValues(typeof(T)).GetValue(0);
            }

            var values = Enum.GetValues(typeof(T));

            return
                (from Enum enumValue in values
                 let charCode = enumValue.ToCharCode()
                 where charCode == code
                 select enumValue).FirstOrDefault();
        }
    }
}

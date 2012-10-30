using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Combinator
{
    public static class EnumHelper
    {
        public static List<T> GetValues<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            IEnumerable<FieldInfo> fields = enumType.GetFields().Where(field => field.IsLiteral);
            return fields.Select(field => field.GetValue(enumType)).Select(value => (T)value).ToList();
        }
    }
}

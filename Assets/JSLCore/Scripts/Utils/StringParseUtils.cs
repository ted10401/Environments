using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSLCore.Utils
{
    /// <summary>
    /// 字串轉型工具
    /// </summary>
    public static class StringParseUtils
    {
        public delegate bool TryParse<T>(string from, out T to);

        /// <summary>
        /// 從字串轉型
        /// </summary>
        public static T ParseValueOrDefault<T>(string str, TryParse<T> tryParse)
        {
            T value;
            if (tryParse(str, out value)) { return value; }
            return default(T);
        }

        /// <summary>
        /// string to bool
        /// </summary>
        public static bool ParseBool(this string self)
        {
            return ParseValueOrDefault<bool>(self, bool.TryParse);
        }

        /// <summary>
        /// string to int
        /// </summary>
        public static int ParseInt(this string self)
        {
            return ParseValueOrDefault<int>(self, int.TryParse);
        }

        /// <summary>
        /// string to uint
        /// </summary>
        public static uint ParseUInt(this string self)
        {
            return ParseValueOrDefault<uint>(self, uint.TryParse);
        }

        /// <summary>
        /// string to float
        /// </summary>
        public static float ParseFloat(this string self)
        {
            return ParseValueOrDefault<float>(self, float.TryParse);
        }

        /// <summary>
        /// string to Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            T result = (T)Enum.Parse(typeof(T), value);
            if (result == null)
            {
                return default(T);
            }

            return result;
        }

    }
}

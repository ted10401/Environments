using System;
using UnityEngine;

namespace JSLCore.Utils
{
    [Serializable]
    public class StringEnum<T> where T : struct, IConvertible
    {
        [SerializeField] private string m_enumString = string.Empty;

        public T GetEnum()
        {
            T result = StringToEnum(m_enumString);

            return result;
        }

        public T StringToEnum(string stringValue)
        {
            if (String.IsNullOrEmpty(stringValue))
            {
                return default(T);
            }

            try
            {
                var result = Enum.Parse(typeof(T), stringValue);

                if (result == null)
                {
                    return default(T);
                }
                else
                {
                    return (T)result;
                }

            }
            catch
            {
                return default(T);
            }
        }
    }
}
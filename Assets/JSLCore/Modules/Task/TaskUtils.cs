using System;

namespace JSLCore.StateManagement
{
    public static class TaskUtils
    {
        public static long GetStateId(params Enum[] values)
        {
            long stateId = 0;

            for (int i = 0; i < values.Length; i++)
            {
                stateId |= GetEnumStateId(values[i]);
            }

            return stateId;
        }

        private static long GetEnumStateId(Enum value)
        {
            return 1 << Convert.ToInt32(value);
        }

        public static long GetAllStateId<T>() where T : struct, IConvertible
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            long stateId = 0;

            for (int i = 0; i < enumValues.Length; i++)
            {
                stateId |= GetEnumStateId(enumValues[i]);
            }

            return stateId;
        }

        private static long GetEnumStateId<T>(T value) where T : struct, IConvertible
        {
            return 1 << Convert.ToInt32(value);
        }
    }
}

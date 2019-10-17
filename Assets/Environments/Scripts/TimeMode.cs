using Sirenix.OdinInspector;

namespace FS2.Environment.Time
{
    public enum TimeMode
    {
        None, //一般模式
        Loop12H, //循環模式 12H
        Loop24H //循環模式 24H
    }

    public class TimeModeValueDropdownList
    {
        public static ValueDropdownList<TimeMode> valueDropdownList = new ValueDropdownList<TimeMode>()
        {
            {  "無循環模式", TimeMode.None },
            {  "全白天循環模式", TimeMode.Loop12H },
            {  "正常循環模式", TimeMode.Loop24H },
        };
    }
}
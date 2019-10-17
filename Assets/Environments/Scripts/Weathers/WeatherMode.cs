using Sirenix.OdinInspector;

namespace FS2.Environment.Weather
{
    public enum WeatherMode
    {
        Sunny, //晴天
        Rainy, //雨天
        Thunderstorm, //雷雨
    }

    public class WeatherModeValueDropdownList
    {
        public static ValueDropdownList<WeatherMode> valueDropdownList = new ValueDropdownList<WeatherMode>()
        {
            {  "晴天", WeatherMode.Sunny },
            {  "雨天", WeatherMode.Rainy },
            {  "雷雨", WeatherMode.Thunderstorm },
        };
    }
}
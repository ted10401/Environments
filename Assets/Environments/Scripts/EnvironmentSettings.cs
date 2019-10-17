using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using FS2.Environment.Time;
using FS2.Environment.Weather;
using FS2.Environment.Weather.Lightning;
using FS2.Environment.Weather.Cloud;

namespace FS2.Environment
{
    [CreateAssetMenu(menuName = "環境設定檔")]
    public class EnvironmentSettings : ScriptableObject
    {
        [FoldoutGroup("時間"), LabelText("時間模式"), ValueDropdown("GetTimeModeValueDropdownList")] public TimeMode timeMode = TimeMode.Loop12H;
        private ValueDropdownList<TimeMode> GetTimeModeValueDropdownList()
        {
            return TimeModeValueDropdownList.valueDropdownList;
        }
        [FoldoutGroup("時間"), LabelText("時間倍數"), HideIf("timeMode", TimeMode.None)] public float timeMultiplier = 360f;
        [FoldoutGroup("時間"), LabelText("初始小時"), Range(0, 24)] public int initialHours;
        [FoldoutGroup("時間"), LabelText("初始分"), Range(0, 60)] public int initialMinutes;
        [FoldoutGroup("時間"), LabelText("初始秒"), Range(0, 60)] public int initialSeconds;
        
        [FoldoutGroup("Direct Light"), LabelText("垂直角度")] public Vector2 directLightVerticalAngle = new Vector2(30f, 75f);
        [FoldoutGroup("Direct Light"), LabelText("顏色")] public Gradient directLightColor;
        [FoldoutGroup("Direct Light"), LabelText("光源強度")] public AnimationCurve directLightIntensity;
        [FoldoutGroup("Direct Light"), LabelText("陰影強度")] public AnimationCurve directLightShadowStrength;
        
        [FoldoutGroup("Ambient Light"), LabelText("Source"), ValueDropdown("GetAmbientModeValueDropdownList")] public AmbientMode ambientMode = AmbientMode.Skybox;
        private ValueDropdownList<AmbientMode> GetAmbientModeValueDropdownList()
        {
            return AmbientModeValueDropdownList.valueDropdownList;
        }
        [FoldoutGroup("Ambient Light"), LabelText("Intensity Multiplier"), ShowIf("ambientMode", AmbientMode.Skybox)] public AnimationCurve ambientIntensity = AnimationCurve.Constant(0f, 1f, 1f);
        [FoldoutGroup("Ambient Light"), LabelText("Sky Color"), ShowIf("ambientMode", AmbientMode.Trilight)] public Gradient ambientSkyColor;
        [FoldoutGroup("Ambient Light"), LabelText("Equator Color"), ShowIf("ambientMode", AmbientMode.Trilight)] public Gradient ambientEquatorColor;
        [FoldoutGroup("Ambient Light"), LabelText("Ground Color"), ShowIf("ambientMode", AmbientMode.Trilight)] public Gradient ambientGroundColor;
        [FoldoutGroup("Ambient Light"), LabelText("Ambient Color"), ShowIf("ambientMode", AmbientMode.Flat)] public Gradient ambientLight;
        
        [FoldoutGroup("天氣"), LabelText("天氣模式"), ValueDropdown("GetWeatherModeValueDropdownList")] public WeatherMode weatherMode = WeatherMode.Sunny;
        private ValueDropdownList<WeatherMode> GetWeatherModeValueDropdownList()
        {
            return WeatherModeValueDropdownList.valueDropdownList;
        }
        [FoldoutGroup("天氣"), LabelText("雲層設定檔")] public EnvironmentCloudSettings environmentCloudSettings;
        [FoldoutGroup("天氣"), LabelText("閃電設定檔"), ShowIf("weatherMode", WeatherMode.Thunderstorm)] public EnvironmentLightningSettings environmentLightningSettings;
    }
}
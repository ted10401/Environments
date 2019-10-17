using JSLCore;
using Sirenix.OdinInspector;
using FS2.Environment.Weather.Cloud;
using FS2.Environment.Weather.Rain;
using FS2.Environment.Weather.Lightning;
using FS2.Environment.Lighting;
using UnityEngine;

namespace FS2.Environment.Weather
{
    [System.Serializable]
    public class EnvironmentWeather : IUpdate
    {
        [LabelText("當前天氣模式"), ValueDropdown("GetWeatherModeValueDropdownList"), OnValueChanged("SetWeatherMode")] public WeatherMode weatherMode = WeatherMode.Sunny;
        private ValueDropdownList<WeatherMode> GetWeatherModeValueDropdownList()
        {
            return WeatherModeValueDropdownList.valueDropdownList;
        }

        [LabelText("當前雲層設定檔"), OnValueChanged("SetEnvironmentCloudSettings", true)] public EnvironmentCloudSettings environmentCloudSettings;
        [LabelText("當前閃電設定檔"), OnValueChanged("SetEnvironmentLightningSettings", true), ShowIf("weatherMode", WeatherMode.Thunderstorm)] public EnvironmentLightningSettings environmentLightningSettings;

        private EnvironmentCloud m_environmentCloud;
        private EnvironmentRain m_environmentRain;
        private EnvironmentLightning m_environmentLightning;

        public EnvironmentWeather(EnvironmentLighting environmentLighting)
        {
            m_environmentCloud = new EnvironmentCloud(environmentLighting);
            m_environmentRain = new EnvironmentRain();
            m_environmentLightning = new EnvironmentLightning();
        }

        public void SetTarget(Transform target)
        {
            m_environmentRain.SetTarget(target);
        }

        public void SetView(EnvironmentView environmentView)
        {
            m_environmentCloud.SetView(environmentView.environmentCloudView);
            m_environmentRain.SetView(environmentView.environmentRainView);
            m_environmentLightning.SetView(environmentView.environmentLightningView);
        }

        public void SetEnvironmentSettings(EnvironmentSettings environmentSettings)
        {
            if(environmentSettings == null)
            {
                return;
            }

            weatherMode = environmentSettings.weatherMode;
            SetWeatherMode(weatherMode);

            environmentCloudSettings = environmentSettings.environmentCloudSettings;
            environmentLightningSettings = environmentSettings.environmentLightningSettings;

            m_environmentCloud.SetEnvironmentSettings(environmentSettings);
            m_environmentLightning.SetEnvironmentSettings(environmentSettings);
        }

        public void SetWeatherMode(WeatherMode weatherMode)
        {
            m_environmentRain.SetWeatherMode(weatherMode);
            m_environmentLightning.SetWeatherMode(weatherMode);
        }

        public void SetEnvironmentCloudSettings(EnvironmentCloudSettings environmentCloudSettings)
        {
            m_environmentCloud.SetEnvironmentCloudSettings(environmentCloudSettings);
        }

        public void SetEnvironmentLightningSettings(EnvironmentLightningSettings environmentLightningSettings)
        {
            m_environmentLightning.SetEnvironmentLightningSettings(environmentLightningSettings);
        }

        public void Update(float deltaTime)
        {
            m_environmentCloud.Update(deltaTime);
            m_environmentRain.Update(deltaTime);
            m_environmentLightning.Update(deltaTime);
        }
    }
}
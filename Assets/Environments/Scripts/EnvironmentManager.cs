using FS2.Environment.Weather;
using JSLCore;
using JSLCore.Resource;
using Sirenix.OdinInspector;
using UnityEngine;
using FS2.Environment.Lighting;

namespace FS2.Environment
{
    public class EnvironmentManager : MonoSingleton<EnvironmentManager>
    {
        private const string VIEW_ASSET_PATH = "Environments/EnvironmentView";

        [SerializeField] private EnvironmentLighting m_environmentLighting;
        [SerializeField] private EnvironmentWeather m_environmentWeather;

        [Title("環境設定檔")]
        [SerializeField, InlineEditor, HideLabel] private EnvironmentSettings m_environmentSettings;
        private bool m_valid = false;

        private bool m_initialized = false;
        private EnvironmentView m_environmentView;

        private void Awake()
        {
            m_environmentLighting = new EnvironmentLighting();
            m_environmentWeather = new EnvironmentWeather(m_environmentLighting);
        }

        public void SetTarget(Transform target)
        {
            m_environmentWeather.SetTarget(target);
        }

        public void ResetEnvironmentSettings()
        {
            m_environmentLighting.Reset();
        }

        public void ClearEnvironmentSettings()
        {
            UpdateEnvironmentSettings(null);
        }

        public void UpdateEnvironmentSettings(EnvironmentSettings environmentSettings)
        {
            m_environmentSettings = environmentSettings;
            Initialize();
        }

        private void Initialize()
        {
            if (m_initialized)
            {
                UpdateEnvironmentSettings();
                return;
            }

            m_initialized = true;
            ResourceManager.Instance.LoadAsync<EnvironmentView>(VIEW_ASSET_PATH, OnEnvironmentComponentLoaded);
        }

        private void OnEnvironmentComponentLoaded(EnvironmentView environmentComponent)
        {
            m_environmentView = GameObject.Instantiate(environmentComponent, transform);
            m_environmentLighting.SetView(m_environmentView);
            m_environmentWeather.SetView(m_environmentView);

            UpdateEnvironmentSettings();
        }

        private void UpdateEnvironmentSettings()
        {
            m_valid = m_environmentSettings != null;

            if(m_environmentView != null)
            {
                m_environmentView.gameObject.SetActive(m_valid);
            }
            
            m_environmentLighting.SetEnvironmentSettings(m_environmentSettings);
            m_environmentWeather.SetEnvironmentSettings(m_environmentSettings);
        }

        private float m_deltaTime;
        private void Update()
        {
            if(!m_valid)
            {
                return;
            }

            m_deltaTime = UnityEngine.Time.deltaTime;
            m_environmentLighting.Update(m_deltaTime);
            m_environmentWeather.Update(m_deltaTime);
        }

        public void SetTime(int hours, int minutes, int seconds)
        {
            if(!m_valid)
            {
                return;
            }

            m_environmentLighting.SetTime(hours, minutes, seconds);
        }

        public void SetWeatherMode(WeatherMode weatherMode)
        {
            if (!m_valid)
            {
                return;
            }

            m_environmentWeather.SetWeatherMode(weatherMode);
        }
    }
}
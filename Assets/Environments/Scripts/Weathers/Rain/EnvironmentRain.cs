using JSLCore;
using UnityEngine;

namespace FS2.Environment.Weather.Rain
{
    public class EnvironmentRain : IUpdate
    {
        private Transform m_target;
        private EnvironmentRainView m_environmentRainView;
        private bool m_active;

        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        public void SetView(EnvironmentRainView environmentRainView)
        {
            m_environmentRainView = environmentRainView;
            m_environmentRainView.SetActive(m_active);
        }

        public void Update(float deltaTime)
        {
            if (m_environmentRainView == null ||
                m_target == null ||
                !m_active)
            {
                return;
            }

            m_environmentRainView.SetPosition(m_target.position);
        }

        public void SetWeatherMode(WeatherMode weatherMode)
        {
            m_active = weatherMode == WeatherMode.Rainy || weatherMode == WeatherMode.Thunderstorm;

            if(m_environmentRainView != null)
            {
                m_environmentRainView.SetActive(m_active);
            }
        }
    }
}
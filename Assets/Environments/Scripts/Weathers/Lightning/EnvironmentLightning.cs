using System.Collections;
using UnityEngine;
using JSLCore;
using JSLCore.Coroutine;

namespace FS2.Environment.Weather.Lightning
{
    public class EnvironmentLightning : IUpdate
    {
        private EnvironmentLightningView m_environmentLightningView;
        private EnvironmentLightningSettings m_environmentLightningSettings;
        private bool m_active;
        private float m_defaultIntensity;
        private float m_timer;
        private bool m_timerOn;
        private CoroutineChain m_coroutineChain;
        private int m_curFlashCount;

        public void SetView(EnvironmentLightningView environmentLightningView)
        {
            m_environmentLightningView = environmentLightningView;
            m_defaultIntensity = m_environmentLightningView.GetIntensity();
        }

        public void SetEnvironmentSettings(EnvironmentSettings environmentSettings)
        {
            if(environmentSettings != null)
            {
                SetEnvironmentLightningSettings(environmentSettings.environmentLightningSettings);
                SetWeatherMode(environmentSettings.weatherMode);
            }
            else
            {
                SetEnvironmentLightningSettings(null);
                SetWeatherMode(false);
            }
        }

        public void SetEnvironmentLightningSettings(EnvironmentLightningSettings environmentLightningSettings)
        {
            m_environmentLightningSettings = environmentLightningSettings;
        }

        public void SetWeatherMode(WeatherMode weatherMode)
        {
            SetWeatherMode(weatherMode == WeatherMode.Thunderstorm);
        }

        private void SetWeatherMode(bool active)
        {
            m_active = active;
            if (m_active)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }

        private void Play()
        {
            Stop();
            if(m_environmentLightningSettings == null)
            {
                return;
            }

            m_timer = Random.Range(m_environmentLightningSettings.lightningTime.x, m_environmentLightningSettings.lightningTime.y);
            m_timerOn = true;
        }

        private void Stop()
        {
            m_timerOn = false;
            if(m_coroutineChain != null)
            {
                m_coroutineChain.StopCoroutine();
                m_coroutineChain = null;
            }
        }

        public void Update(float deltaTime)
        {
            if (m_environmentLightningView == null ||
                m_environmentLightningSettings == null ||
                !m_timerOn)
            {
                return;
            }

            m_timer -= deltaTime;
            if (m_timer <= 0)
            {
                m_coroutineChain = CoroutineManager.Instance.Create()
                    .Enqueue(Lightning())
                    .StartCoroutine();
            }
        }

        private IEnumerator Lightning()
        {
            m_timerOn = false;

            m_environmentLightningView.SetEulerAngles(new Vector3(Random.Range(m_environmentLightningSettings.lightningVerticalAngle.x, m_environmentLightningSettings.lightningVerticalAngle.y), Random.Range(0f, 360f), 0));
            m_environmentLightningView.SetLightEnabled(true);
            m_curFlashCount = Random.Range(m_environmentLightningSettings.lightningCount.x, m_environmentLightningSettings.lightningCount.y);

            while (m_curFlashCount > 0)
            {
                m_environmentLightningView.SetIntensity(m_defaultIntensity * Random.Range(m_environmentLightningSettings.lightningIntensity.x, m_environmentLightningSettings.lightningIntensity.y));
                yield return CoroutineUtils.WaitForSeconds(Random.Range(m_environmentLightningSettings.lightningInterval.x, m_environmentLightningSettings.lightningInterval.y));
                m_environmentLightningView.SetIntensity(m_defaultIntensity);
                m_curFlashCount--;
            }

            m_environmentLightningView.SetLightEnabled(false);
            m_timer = Random.Range(m_environmentLightningSettings.lightningTime.x, m_environmentLightningSettings.lightningTime.y);
            m_timerOn = true;

            m_coroutineChain.StopCoroutine();
            m_coroutineChain = null;
        }
    }
}
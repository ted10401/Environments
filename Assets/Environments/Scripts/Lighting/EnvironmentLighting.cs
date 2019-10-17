using UnityEngine;
using JSLCore;
using FS2.Environment.Time;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace FS2.Environment.Lighting
{
    [System.Serializable]
    public class EnvironmentLighting : IUpdate
    {
        [LabelText("當前時間模式"), ValueDropdown("GetTimeModeValueDropdownList")] public TimeMode timeMode = TimeMode.Loop12H;
        private ValueDropdownList<TimeMode> GetTimeModeValueDropdownList()
        {
            return TimeModeValueDropdownList.valueDropdownList;
        }
        [LabelText("當前小時"), Range(0, 24), OnValueChanged("UpdateTimeLerp")] public int hours;
        [LabelText("當前分"), Range(0, 60), OnValueChanged("UpdateTimeLerp")] public int minutes;
        [LabelText("當前秒"), Range(0, 60), OnValueChanged("UpdateTimeLerp")] public int seconds;
        [LabelText("當前時間插值"), Range(0f, 1f), OnValueChanged("UpdateLight")] public float timeLerp;
        [LabelText("當前水平角度插值"), ReadOnly] private float m_horizontalAngleLerp;
        [LabelText("當前水平角度"), ReadOnly] private float m_directLightHorizontalAngle;
        [LabelText("當前垂直角度插值"), ReadOnly] private float m_verticalAngleLerp;
        [LabelText("當前垂直角度"), ReadOnly] private float m_directLightVerticalAngle;
        [LabelText("當前顏色插值"), ReadOnly] private float m_colorLerp;
        [LabelText("當前顏色"), ReadOnly] private Color m_directLightColor;
        [LabelText("當前光源強度插值"), ReadOnly] private float m_intensityLerp;
        [LabelText("當前光源強度"), ReadOnly] private float m_directLightIntensity;
        [LabelText("當前陰影強度插值"), ReadOnly] private float m_shadowStrengthLerp;
        [LabelText("當前陰影強度"), ReadOnly] private float m_directLightShadowStrength;
        [LabelText("當前 Ambient Light 插植"), ReadOnly] private float m_ambientLightLerp;

        private EnvironmentLightingView m_environmentLightingView;
        private EnvironmentSettings m_environmentSettings;
        private TimeMode m_resetTimeMode;
        private int m_resetHours;
        private int m_resetMinutes;
        private int m_resetSeconds;
        private float m_deltaTime;
        private int m_deltaSeconds;
        private float m_lastTimeLerp;

        public void Reset()
        {
            timeMode = m_resetTimeMode;
            hours = m_resetHours;
            minutes = m_resetMinutes;
            seconds = m_resetSeconds;
        }

        public void SetView(EnvironmentView environmentView)
        {
            m_environmentLightingView = environmentView.environmentLightingView;
        }

        public void SetEnvironmentSettings(EnvironmentSettings environmentSettings)
        {
            m_environmentSettings = environmentSettings;
            if(m_environmentSettings != null)
            {
                UpdateTime();
            }
        }

        private void UpdateTime()
        {
            timeMode = m_environmentSettings.timeMode;
            hours = m_environmentSettings.initialHours;
            minutes = m_environmentSettings.initialMinutes;
            seconds = m_environmentSettings.initialSeconds;
            UpdateTimeLerp();
        }

        public void SetTime(int hours, int minutes, int seconds)
        {
            m_resetTimeMode = timeMode;
            m_resetHours = hours;
            m_resetMinutes = minutes;
            m_resetSeconds = seconds;

            timeMode = TimeMode.None;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            UpdateTimeLerp();
        }

        public void Update(float deltaTime)
        {
            if (m_environmentLightingView == null ||
                m_environmentSettings == null ||
                timeMode == TimeMode.None)
            {
                return;
            }

            m_deltaTime += deltaTime * m_environmentSettings.timeMultiplier;
            m_deltaSeconds = (int)m_deltaTime;
            m_deltaTime -= m_deltaSeconds;

            UpdateSeconds(m_deltaSeconds);
            UpdateTimeLerp();
        }

        private void UpdateSeconds(int deltaSeconds)
        {
            seconds += deltaSeconds;
            while (seconds >= 60)
            {
                seconds -= 60;
                UpdateMinutes();
            }
        }

        private void UpdateMinutes()
        {
            minutes += 1;
            if (minutes >= 60)
            {
                minutes -= 60;
                UpdateHours();
            }
        }

        private void UpdateHours()
        {
            hours += 1;
            if (hours >= 24)
            {
                hours -= 24;
            }
        }

        private void UpdateTimeLerp()
        {
            timeLerp = (float)(hours * 3600 + minutes * 60 + seconds) / 86400;
            timeLerp = Mathf.Clamp01(timeLerp);
            UpdateLight();
        }

        private void UpdateLight()
        {
            if (m_lastTimeLerp == timeLerp)
            {
                return;
            }

            m_lastTimeLerp = timeLerp;
            UpdateDirectLight(m_lastTimeLerp);
        }

        private void UpdateDirectLight(float lerp)
        {
            if(m_environmentSettings == null || m_environmentLightingView == null)
            {
                return;
            }

            UpdateDirectLightAngle(lerp);
            UpdateColorLerp(lerp);
            UpdateDirectLightColor();
            UpdateDirectLightIntensity();
            UpdateDirectLightShadowStrength();
            UpdateAmbientLight();
        }

        private void UpdateDirectLightAngle(float lerp)
        {
            //Horizontal Angle
            //0.25 ~ 0.75 => 0.00 ~ 0.50 =>   0 ~ 180
            //0.75 ~ 1.00 => 0.50 ~ 0.75 => 180 ~ 270
            //0.00 ~ 0.25 => 0.75 ~ 1.00 => 270 ~ 360

            m_horizontalAngleLerp = lerp - 0.25f;
            if (m_horizontalAngleLerp < 0)
            {
                m_horizontalAngleLerp += 1f;
            }
            m_directLightHorizontalAngle = Mathf.Lerp(0, 360, m_horizontalAngleLerp);
            m_environmentLightingView.SetHorizontalParentLocalEulerAngles(new Vector3(0, m_directLightHorizontalAngle, 0));

            //Vertical Angle
            //0.00 ~ 0.25 => 1.0 ~ 0.0 => 90 ~ 30
            //0.25 ~ 0.50 => 0.0 ~ 1.0 => 30 ~ 90
            //0.50 ~ 0.75 => 1.0 ~ 0.0 => 90 ~ 30
            //0.75 ~ 1.00 => 0.0 ~ 1.0 => 30 ~ 90

            if (lerp <= 0.25f)
            {
                m_verticalAngleLerp = lerp * 4;
                m_directLightVerticalAngle = Mathf.Lerp(m_environmentSettings.directLightVerticalAngle.y, m_environmentSettings.directLightVerticalAngle.x, lerp * 4);
            }
            else if (lerp <= 0.5f)
            {
                m_verticalAngleLerp = (lerp - 0.25f) * 4;
                m_directLightVerticalAngle = Mathf.Lerp(m_environmentSettings.directLightVerticalAngle.x, m_environmentSettings.directLightVerticalAngle.y, (lerp - 0.25f) * 4);
            }
            else if (lerp <= 0.75f)
            {
                m_verticalAngleLerp = (lerp - 0.5f) * 4;
                m_directLightVerticalAngle = Mathf.Lerp(m_environmentSettings.directLightVerticalAngle.y, m_environmentSettings.directLightVerticalAngle.x, (lerp - 0.5f) * 4);
            }
            else
            {
                m_verticalAngleLerp = (lerp - 0.75f) * 4;
                m_directLightVerticalAngle = Mathf.Lerp(m_environmentSettings.directLightVerticalAngle.x, m_environmentSettings.directLightVerticalAngle.y, (lerp - 0.75f) * 4);
            }

            m_environmentLightingView.SetVerticalParentLocalEulerAngles(new Vector3(m_directLightVerticalAngle, 0, 0));
        }

        private void UpdateColorLerp(float lerp)
        {
            m_colorLerp = lerp;
            if (timeMode == TimeMode.Loop12H)
            {
                if (m_colorLerp < 0.25f)
                {
                    m_colorLerp = 0.25f + (0.25f - m_colorLerp);
                }
                else if (m_colorLerp > 0.75f)
                {
                    m_colorLerp = 0.75f - (m_colorLerp - 0.75f);
                }
            }
        }

        private void UpdateDirectLightColor()
        {
            m_directLightColor = m_environmentSettings.directLightColor.Evaluate(m_colorLerp);
            m_environmentLightingView.SetLightColor(m_directLightColor);
        }

        private void UpdateDirectLightIntensity()
        {
            m_intensityLerp = m_colorLerp;
            m_directLightIntensity = m_environmentSettings.directLightIntensity.Evaluate(m_intensityLerp);
            m_environmentLightingView.SetLightIntensity(m_directLightIntensity);
        }

        private void UpdateDirectLightShadowStrength()
        {
            m_shadowStrengthLerp = m_colorLerp;
            m_directLightShadowStrength = m_environmentSettings.directLightShadowStrength.Evaluate(m_shadowStrengthLerp);
            m_environmentLightingView.SetLightShadowStrength(m_directLightShadowStrength);
        }

        private void UpdateAmbientLight()
        {
            m_ambientLightLerp = m_colorLerp;
            if(RenderSettings.ambientMode != m_environmentSettings.ambientMode)
            {
                RenderSettings.ambientMode = m_environmentSettings.ambientMode;
            }
            
            switch (m_environmentSettings.ambientMode)
            {
                case AmbientMode.Skybox:
                    RenderSettings.ambientIntensity = m_environmentSettings.ambientIntensity.Evaluate(m_ambientLightLerp);
                    break;
                case AmbientMode.Trilight:
                    RenderSettings.ambientSkyColor = m_environmentSettings.ambientSkyColor.Evaluate(m_ambientLightLerp);
                    RenderSettings.ambientEquatorColor = m_environmentSettings.ambientEquatorColor.Evaluate(m_ambientLightLerp);
                    RenderSettings.ambientGroundColor = m_environmentSettings.ambientGroundColor.Evaluate(m_ambientLightLerp);
                    break;
                case AmbientMode.Flat:
                    RenderSettings.ambientLight = m_environmentSettings.ambientLight.Evaluate(m_ambientLightLerp);
                    break;
            }
        }

        public Color GetAmbientColor()
        {
            switch (m_environmentSettings.ambientMode)
            {
                case AmbientMode.Skybox:
                    return RenderSettings.subtractiveShadowColor;
                case AmbientMode.Trilight:
                    return (RenderSettings.ambientSkyColor + RenderSettings.ambientEquatorColor) / 2;
                case AmbientMode.Flat:
                    return RenderSettings.ambientLight;
            }

            return Color.black;
        }
    }
}
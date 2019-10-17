using UnityEngine;
using JSLCore;
using FS2.Environment.Lighting;

namespace FS2.Environment.Weather.Cloud
{
    public class EnvironmentCloud : IUpdate
    {
        private EnvironmentLighting m_environmentLighting;
        private UnityEngine.Camera m_camera;
        private Transform m_cameraTransform;
        private EnvironmentCloudView m_environmentCloudView;
        private EnvironmentSettings m_environmentSettings;
        private EnvironmentCloudSettings m_environmentCloudSettings;

        public EnvironmentCloud(EnvironmentLighting environmentLighting)
        {
            m_environmentLighting = environmentLighting;
            m_camera = UnityEngine.Camera.main;
            m_cameraTransform = m_camera.transform;
        }

        public void SetView(EnvironmentCloudView environmentCloudView)
        {
            m_environmentCloudView = environmentCloudView;
            UpdateQuadScale();
            UpdateEnvironmentCloudSettings();
        }

        private void UpdateQuadScale()
        {
            if(m_camera == null || m_environmentCloudView == null)
            {
                return;
            }

            float height = Mathf.Tan(m_camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * m_camera.farClipPlane * 2f;
            m_environmentCloudView.SetScale(new Vector3(height * m_camera.aspect, height, 0f));
        }

        public void SetEnvironmentSettings(EnvironmentSettings environmentSettings)
        {
            m_environmentSettings = environmentSettings;
            if(m_environmentSettings == null)
            {
                return;
            }

            SetEnvironmentCloudSettings(m_environmentSettings.environmentCloudSettings);
        }

        public void SetEnvironmentCloudSettings(EnvironmentCloudSettings environmentCloudSettings)
        {
            m_environmentCloudSettings = environmentCloudSettings;
            UpdateEnvironmentCloudSettings();
        }

        private void UpdateEnvironmentCloudSettings()
        {
            if (m_environmentCloudSettings != null && m_environmentCloudView != null)
            {
                m_environmentCloudView.SetCloudTexture(m_environmentCloudSettings.cloudTexture);
                m_environmentCloudView.SetCloudFactor(m_environmentCloudSettings.cloudFactor);

                if (m_environmentCloudSettings.useCustomsShadowColor)
                {
                    m_environmentCloudView.SetShadowColor(m_environmentCloudSettings.customShadowColor);
                }
                else
                {
                    m_environmentCloudView.SetShadowColor(m_environmentLighting.GetAmbientColor());
                }
            }
        }

        public void Update(float deltaTime)
        {
            if (m_camera == null ||
                m_environmentCloudView == null ||
                m_environmentCloudSettings == null)
            {
                return;
            }
            
            UpdateQuad();
            UpdateShadowStrength(deltaTime);
        }

        private void UpdateQuad()
        {
            m_environmentCloudView.SetPositionAndRotation(m_cameraTransform.position + m_cameraTransform.forward * (m_camera.farClipPlane - 0.1f), m_cameraTransform.rotation);
        }

        private float m_shadowStrengthTimer;
        private void UpdateShadowStrength(float deltaTime)
        {
            m_shadowStrengthTimer += deltaTime * m_environmentCloudSettings.shadowTimeMultiply;
            if(m_shadowStrengthTimer > 1)
            {
                m_shadowStrengthTimer = 0f;
            }
            
            m_environmentCloudView.SetShadowStrength(m_environmentCloudSettings.shadowStrengthCurve.Evaluate(m_shadowStrengthTimer));
        }
    }
}
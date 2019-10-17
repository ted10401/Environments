using Sirenix.OdinInspector;
using UnityEngine;

namespace FS2.Environment.Lighting
{
    public class EnvironmentLightingView : MonoBehaviour
    {
        [LabelText("水平軸"), SerializeField] private Transform m_horizontalParent = null;
        [LabelText("垂直軸"), SerializeField] private Transform m_verticalParent = null;
        [SerializeField] private Light m_light = null;

        public void SetHorizontalParentLocalEulerAngles(Vector3 localEulerAngles)
        {
            m_horizontalParent.localEulerAngles = localEulerAngles;
        }

        public void SetVerticalParentLocalEulerAngles(Vector3 localEulerAngles)
        {
            m_verticalParent.localEulerAngles = localEulerAngles;
        }

        public void SetLightColor(Color color)
        {
            m_light.color = color;
        }

        public void SetLightIntensity(float intensity)
        {
            m_light.intensity = intensity;
            m_light.enabled = intensity > 0;
        }

        public void SetLightShadowStrength(float shadowStrength)
        {
            m_light.shadowStrength = shadowStrength;
            m_light.shadows = shadowStrength > 0 ? LightShadows.Soft : LightShadows.None;
        }
    }
}
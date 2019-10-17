using UnityEngine;

namespace FS2.Environment.Weather.Lightning
{
    public class EnvironmentLightningView : MonoBehaviour
    {
        [SerializeField] private Light m_light = null;

        public void SetEulerAngles(Vector3 eulerAngles)
        {
            m_light.transform.eulerAngles = eulerAngles;
        }

        public void SetLightEnabled(bool enabled)
        {
            m_light.enabled = enabled;
        }

        public float GetIntensity()
        {
            return m_light.intensity;
        }

        public void SetIntensity(float intensity)
        {
            m_light.intensity = intensity;
        }
    }
}
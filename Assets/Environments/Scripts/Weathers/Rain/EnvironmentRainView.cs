using UnityEngine;

namespace FS2.Environment.Weather.Rain
{
    public class EnvironmentRainView : MonoBehaviour
    {
        [SerializeField] private Transform m_rain = null;

        public void SetPosition(Vector3 position)
        {
            m_rain.position = position;
        }

        public void SetActive(bool active)
        {
            m_rain.gameObject.SetActive(active);
        }
    }
}
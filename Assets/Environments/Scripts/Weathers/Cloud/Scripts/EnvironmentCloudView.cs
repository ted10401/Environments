using UnityEngine;

namespace FS2.Environment.Weather.Cloud
{
    public class EnvironmentCloudView : MonoBehaviour
    {
        private readonly int CLOUD_TEXTURE_PROPERTY_ID = Shader.PropertyToID("_CloudTexture");
        private readonly int CLOUD_FACTOR_PROPERTY_ID = Shader.PropertyToID("_CloudFactor");
        private readonly int CLOUD_SCALE_PROPERTY_ID = Shader.PropertyToID("_CloudScale");
        private readonly int CLOUD_ALPHA_PROPERTY_ID = Shader.PropertyToID("_CloudAlpha");
        private readonly int CLOUD_COVER_PROPERTY_ID = Shader.PropertyToID("_CloudCover");
        private readonly int SHADOW_STRENGTH_PROPERTY_ID = Shader.PropertyToID("_ShadowStrength");
        private readonly int SHADOW_COLOR_PROPERTY_ID = Shader.PropertyToID("_ShadowColor");

        [SerializeField] private Renderer m_renderer = null;
        private Transform m_transform;
        private Material m_material;

        private void Awake()
        {
            m_transform = m_renderer.transform;
            m_material = m_renderer.material;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            if(m_transform == null)
            {
                return;
            }

            m_transform.SetPositionAndRotation(position, rotation);
        }

        public void SetScale(Vector3 scale)
        {
            if (m_transform == null)
            {
                return;
            }

            m_transform.localScale = scale;
        }

        public void SetCloudTexture(Texture value)
        {
            m_material.SetTexture(CLOUD_TEXTURE_PROPERTY_ID, value);
        }

        public void SetCloudFactor(Vector4 value)
        {
            m_material.SetVector(CLOUD_FACTOR_PROPERTY_ID, value);
        }

        public void SetCloudScale(float value)
        {
            m_material.SetFloat(CLOUD_SCALE_PROPERTY_ID, value);
        }

        public void SetCloudAlpha(float value)
        {
            m_material.SetFloat(CLOUD_ALPHA_PROPERTY_ID, value);
        }

        public void SetCloudCover(float value)
        {
            m_material.SetFloat(CLOUD_COVER_PROPERTY_ID, value);
        }

        public void SetShadowStrength(float value)
        {
            m_material.SetFloat(SHADOW_STRENGTH_PROPERTY_ID, value);
        }

        public void SetShadowColor(Color value)
        {
            m_material.SetColor(SHADOW_COLOR_PROPERTY_ID, value);
        }
    }
}
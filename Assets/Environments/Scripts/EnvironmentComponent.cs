using UnityEngine;
using Sirenix.OdinInspector;

namespace FS2.Environment
{
    public class EnvironmentComponent : MonoBehaviour
    {
        [Title("環境設定檔")]
        [SerializeField, InlineEditor, HideLabel] private EnvironmentSettings m_environmentSettings = null;

        private void Awake()
        {
            EnvironmentManager.Instance.UpdateEnvironmentSettings(m_environmentSettings);
        }
    }
}
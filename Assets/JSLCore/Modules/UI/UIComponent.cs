using UnityEngine;
using UnityEngine.UI;

namespace JSLCore.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIComponent : MonoBehaviour, IDestroy
    {
        public bool active { get; private set; } = true;
        [SerializeField] protected Canvas m_canvas;
        [SerializeField] protected GraphicRaycaster m_graphicRaycaster;
        [SerializeField] protected CanvasGroup m_canvasGroup;
        [SerializeField] protected ScrollRect[] m_scrollRects;
        private Transform m_transform;

        [ContextMenu("Reset UIComponent")]
        private void ResetUIComponent()
        {
            m_canvas = GetComponent<Canvas>();
            m_graphicRaycaster = GetComponent<GraphicRaycaster>();
            m_canvasGroup = GetComponent<CanvasGroup>();
            m_scrollRects = GetComponentsInChildren<ScrollRect>(true);
        }

        protected virtual void Awake()
        {
            m_transform = transform;
        }

        public void SetCanvas(Camera camera)
        {
            if(m_canvas != null)
            {
                m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
                m_canvas.worldCamera = camera;
            }
        }

        public void SetCanvasPriority(int priority)
        {
            if (m_canvas != null)
            {
                m_canvas.sortingOrder = priority;
            }
        }

        public void SetActiveInternal(bool active)
        {
            if (this.active == active)
            {
                return;
            }

            this.active = active;
            SetActive(this.active);
        }

        protected virtual void SetActive(bool active)
        {
            if(m_graphicRaycaster != null)
            {
                m_graphicRaycaster.enabled = active;
            }

            if(m_canvasGroup != null)
            {
                m_canvasGroup.alpha = active ? 1 : 0;
                m_canvasGroup.blocksRaycasts = active;
            }

            if(m_scrollRects != null && m_scrollRects.Length > 0)
            {
                for(int i = 0; i < m_scrollRects.Length; i++)
                {
                    if(m_scrollRects[i] == null)
                    {
                        continue;
                    }

                    m_scrollRects[i].enabled = active;
                }
            }

            if(active)
            {
                m_transform.SetAsLastSibling();
            }
            else
            {
                m_transform.SetAsFirstSibling();
            }
        }

        public virtual void Destroy() { }
    }
}
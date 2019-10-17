using JSLCore.StateManagement;

namespace JSLCore.UI
{
    public abstract class UITask<T> : Task where T : UIComponent
    {
        protected virtual string assetPath { get { return string.Empty; } }
        protected T m_view;
        protected bool m_enable = false;

        public UITask() : base()
        {
            if (m_view == null)
            {
                UIManager.Instance.LoadUIAsync<T>(assetPath, OnViewLoaded);
            }
        }

        protected virtual void OnViewLoaded(T view)
        {
            m_view = view;
            Show(active);
        }

        public override void Show(bool show)
        {
            UIManager.Instance.SetActive(m_view, show);
        }

        public override void Update(float deltaTime)
        {

        }

        public override void FixedUpdate(float deltaTime)
        {

        }

        public override void LateUpdate(float deltaTime)
        {

        }

        public override void Destroy()
        {
            base.Destroy();
            UIManager.Instance.Destroy(m_view);
        }
    }
}
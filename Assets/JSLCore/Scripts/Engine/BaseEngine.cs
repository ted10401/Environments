using UnityEngine;
using JSLCore.StateManagement;

namespace JSLCore
{
    public abstract class BaseEngine : MonoBehaviour, IInitialize
	{
        public static bool valid;
        protected StateManager m_stateManager;

		private void Awake()
		{
            if(valid)
            {
                InvalidInitialize();
                return;
            }

            valid = true;
            m_stateManager = new StateManager();
            Initialize();
		}

        public virtual void InvalidInitialize() { }
        public abstract void Initialize();

		public virtual void Update()
		{
			if (null != m_stateManager.currentState)
			{
				m_stateManager.Update (Time.deltaTime);
			}
		}

        public virtual void FixedUpdate()
        {
            if (null != m_stateManager.currentState)
            {
                m_stateManager.FixedUpdate(Time.fixedDeltaTime);
            }
        }

        public virtual void LateUpdate()
        {
            if (null != m_stateManager.currentState)
            {
                m_stateManager.LateUpdate(Time.deltaTime);
            }
        }
    }
}
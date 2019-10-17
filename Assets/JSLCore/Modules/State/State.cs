using JSLCore.Event;

namespace JSLCore.StateManagement
{
    public abstract class State : EventListener, IUpdate, IFixedUpdate, ILateUpdate
	{
		public StateManager stateManager { get; private set; }
		public TaskManager taskManager { get; private set; }

        protected State(StateManager stateManager) : base()
		{
			this.stateManager = stateManager;
			taskManager = new TaskManager(stateManager);
		}

		public override void Destroy ()
		{
			base.Destroy ();
			taskManager.Destroy();
		}

		#region IUpdate
		public virtual void Update (float deltaTime)
		{
			taskManager.Update(deltaTime);
		}
        #endregion

        #region IFixedUpdate
        public virtual void FixedUpdate(float deltaTime)
        {
            taskManager.FixedUpdate(deltaTime);
        }
        #endregion

        #region ILateUpdate
        public virtual void LateUpdate(float deltaTime)
        {
            taskManager.LateUpdate(deltaTime);
        }
        #endregion
    }
}
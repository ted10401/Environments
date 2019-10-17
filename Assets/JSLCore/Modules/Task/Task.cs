using JSLCore.Event;

namespace JSLCore.StateManagement
{
    public abstract class Task : EventListener, IUpdate, IFixedUpdate, ILateUpdate
    {
        public TaskManager taskManager;
        public StateManager stateManager { get { return taskManager.stateManager; } }

        protected Task() : base()
        {

        }

        protected override void OnActiveChanged()
        {
            Show(active);
        }

        public override void Destroy()
        {
            taskManager = null;
            base.Destroy();
        }

        public abstract void Show(bool show);
        public abstract void Update(float deltaTime);
        public abstract void FixedUpdate(float deltaTime);
        public abstract void LateUpdate(float deltaTime);
    }
}

namespace JSLCore.StateManagement
{
    public class StateManager : IUpdate, IFixedUpdate, ILateUpdate
    {
        public bool isPause { get; set; }
        public State currentState { get; private set; }

        public StateManager()
        {
            isPause = false;
            currentState = null;
        }

        public void ChangeState(State newState)
        {
            if (currentState != null)
            {
                currentState.Destroy();
            }

            currentState = newState;
        }

        #region IUpdate
        public void Update(float deltaTime)
        {
            if (currentState != null && !isPause)
            {
                currentState.Update(deltaTime);
            }
        }
        #endregion

        #region IFixedUpdate
        public void FixedUpdate(float deltaTime)
        {
            if (currentState != null && !isPause)
            {
                currentState.FixedUpdate(deltaTime);
            }
        }
        #endregion

        #region ILateUpdate
        public void LateUpdate(float deltaTime)
        {
            if (currentState != null && !isPause)
            {
                currentState.LateUpdate(deltaTime);
            }
        }
        #endregion
    }
}
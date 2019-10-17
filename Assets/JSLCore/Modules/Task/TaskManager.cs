using System.Collections.Generic;
using System;

namespace JSLCore.StateManagement
{
    public class TaskManager : IUpdate, IFixedUpdate, ILateUpdate, IDestroy
    {
        public StateManager stateManager { get; private set; }
        public long currentState
        {
            get { return m_currentState; }
            private set
            {
                m_currentState = value;
                onStateChanged?.Invoke();
            }
        }
        private long m_currentState;
        public Action onStateChanged;

        private class TaskData
        {
            public Task task;
            public long activeStates;
        }

        private List<TaskData> m_tasks;
        private List<TaskData> m_activeTasks;

        public TaskManager(StateManager stateManager)
        {
            m_tasks = new List<TaskData>();
            m_activeTasks = new List<TaskData>();

            this.stateManager = stateManager;
        }

        public void AddTask(Task task, params Enum[] activeStates)
        {
            task.taskManager = this;

            TaskData td = new TaskData();
            td.task = task;
            td.activeStates = TaskUtils.GetStateId(activeStates);

            m_tasks.Add(td);
        }

        public void AddTask(Task task, long activeStates)
        {
            task.taskManager = this;

            TaskData td = new TaskData();
            td.task = task;
            td.activeStates = activeStates;

            m_tasks.Add(td);
        }

        public void ChangeState(Enum activeState)
        {
            currentState = TaskUtils.GetStateId(activeState);
            m_activeTasks.Clear();

            for (int i = 0; i < m_tasks.Count; i++)
            {
                m_tasks[i].task.active = ContainState(m_tasks[i].activeStates, currentState);

                if (m_tasks[i].task.active)
                {
                    m_activeTasks.Add(m_tasks[i]);
                }
            }

            onStateChanged?.Invoke();
        }

        private static bool ContainState(long stateFlags, long stateId)
        {
            return (stateFlags & stateId) == stateId;
        }

        #region IUpdate
        public void Update(float deltaTime)
        {
            for (int i = 0; i < m_activeTasks.Count; i++)
            {
                m_activeTasks[i].task.Update(deltaTime);
            }
        }
        #endregion

        #region IFixedUpdate
        public void FixedUpdate(float deltaTime)
        {
            for (int i = 0; i < m_activeTasks.Count; i++)
            {
                m_activeTasks[i].task.FixedUpdate(deltaTime);
            }
        }
        #endregion

        #region ILateUpdate
        public void LateUpdate(float deltaTime)
        {
            for (int i = 0; i < m_activeTasks.Count; i++)
            {
                m_activeTasks[i].task.LateUpdate(deltaTime);
            }
        }
        #endregion

        #region IDestroyable
        public void Destroy()
        {
            for (int i = 0; i < m_tasks.Count; i++)
            {
                m_tasks[i].task.Destroy();
            }

            m_tasks.Clear();
            m_activeTasks.Clear();
        }
        #endregion
    }
}

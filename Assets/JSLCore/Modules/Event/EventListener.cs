using System.Collections.Generic;

namespace JSLCore.Event
{
	public class EventListener : IEventListener, IDestroy
	{
        public delegate EventResult EventCallback(object eventData);

		protected class EventListenerData
		{
			public EventCallback eventCallback;
			public bool callWhenInactive;
		}

        protected Dictionary<int, EventListenerData> m_eventListeners;

		private bool m_active;
		public bool active
		{
			get { return m_active; }
			set
			{
				m_active = value;
				OnActiveChanged();
			}
		}

		protected virtual void OnActiveChanged()
		{

		}

		public EventListener()
		{
            active = true;
            m_eventListeners = new Dictionary<int, EventListenerData>();
		}

        public void ListenForEvent(int eventName, EventCallback callback, bool callWhenInactive = false, int priority = 0)
		{
            EventListenerData eventListenerData = new EventListenerData
            {
                eventCallback = callback,
                callWhenInactive = callWhenInactive
            };

            m_eventListeners[eventName] = eventListenerData;

			EventManager.Instance.RegisterListener(eventName, this, priority);
		}

        public void StopListenForEvent(int eventName)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				m_eventListeners.Remove(eventName);

                EventManager.Instance.RemoveListener(eventName, this);
			}
		}

		#region IEventListener
        public EventResult OnEvent(int eventName, object eventData)
		{
			if(m_eventListeners.ContainsKey(eventName))
			{
				EventListenerData eventListenerData = m_eventListeners[eventName];
				
				if(!active && !eventListenerData.callWhenInactive)
				{
					return null;
				}
				
				if(eventListenerData.eventCallback != null)
				{
					return eventListenerData.eventCallback(eventData);
				}
			}
			
			return null;
		}
		#endregion

		#region IDestroyable
		public virtual void Destroy()
		{
            RemoveListener();

        }

        protected void RemoveListener()
        {
            foreach (int eventName in m_eventListeners.Keys)
            {
                EventManager.Instance.RemoveListener(eventName, this);
            }

            m_eventListeners.Clear();
        }
		#endregion
	}
}
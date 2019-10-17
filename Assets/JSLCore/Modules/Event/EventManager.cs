using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using System.Text;
#endif

namespace JSLCore.Event
{
    public class EventManager : Singleton<EventManager>
	{
        private class ListenerContainer : IComparable<ListenerContainer>
		{
			public IEventListener eventListener { get; private set; }
			public int priority { get; private set; }

			public ListenerContainer(IEventListener eventListener, int priority)
			{
				this.eventListener = eventListener;
				this.priority = priority;
			}

            public int CompareTo(ListenerContainer listener)
            {
                if (null == listener)
                {
                    return 0;
                }
                else
                {
                    return listener.priority.CompareTo(this.priority);
                }
            }
		}

        private Dictionary<int, List<ListenerContainer>> m_eventListenerContainers;
        private readonly Dictionary<int, ListenerContainer[]> m_eventListenerArrays;

        private ListenerContainer m_cacheListenerContainer;
        private List<ListenerContainer> m_cacheRemoveContainer = new List<ListenerContainer>();
        private List<ListenerContainer> m_cacheTempContainer;
        public EventManager()
		{
            m_eventListenerContainers = new Dictionary<int, List<ListenerContainer>>();
            m_eventListenerArrays = new Dictionary<int, ListenerContainer[]>();
		}

        public void RegisterListener(int eventId, IEventListener listener, int priority = 0)
		{
			if(!m_eventListenerContainers.ContainsKey(eventId))
			{
				m_eventListenerContainers[eventId] = new List<ListenerContainer>();
			}

			List<ListenerContainer> listeners = m_eventListenerContainers[eventId];
            int listenerCount = listeners.Count;
            for (int i = 0; i < listenerCount; i++)
            {
                if (listeners[i].eventListener == listener)
                {
                    JSLDebug.LogException(new Exception("[EventManager] - Listener is already registered for this object."));
                    return;
                }
            }

			listeners.Add(new ListenerContainer(listener, priority));
            listeners.Sort();

            m_eventListenerContainers[eventId] = listeners;

            if (m_eventListenerArrays.ContainsKey(eventId))
            {
                m_eventListenerArrays[eventId] = listeners.ToArray();
            }
            else
            {
                m_eventListenerArrays.Add(eventId, listeners.ToArray());
            }
		}

        public void RemoveListener(int eventId, IEventListener listener)
		{
            m_cacheTempContainer = null;
            m_cacheListenerContainer = null;
            m_cacheRemoveContainer.Clear();
            if (m_eventListenerContainers.TryGetValue(eventId, out m_cacheTempContainer))
            {
                for (int i = 0, Count = m_cacheTempContainer.Count; i < Count; i++)
                {
                    m_cacheListenerContainer = m_cacheTempContainer[i];

                    if (m_cacheListenerContainer.eventListener == listener)
                    {
                        m_cacheRemoveContainer.Add(m_cacheListenerContainer);
                    }
                }

                for (int i = 0, Count = m_cacheRemoveContainer.Count; i < Count; i++)
                {
                    m_cacheTempContainer.Remove(m_cacheRemoveContainer[i]);
                }

                if (m_eventListenerArrays.ContainsKey(eventId))
                {
                    m_eventListenerArrays[eventId] = m_cacheTempContainer.ToArray();
                }
                else
                {
                    m_eventListenerArrays.Add(eventId, m_cacheTempContainer.ToArray());
                }
            }
		}

        public EventResult SendEvent(int eventId, object eventData = null)
        {
            ListenerContainer[] containers;
            if (m_eventListenerArrays.TryGetValue(eventId, out containers))
            {
                EventResult result;
                int listenerCount = containers.Length;

                for (int i = 0; i < listenerCount; i++)
                {
                    if (containers[i] == null) continue;

                    result = containers[i].eventListener.OnEvent(eventId, eventData);

                    if (null == result)
                    {
                        continue;
                    }

                    return result;
                }
            }

#if UNITY_EDITOR
            SetEditorData(eventId , containers);
#endif

            return null;
        }

#if UNITY_EDITOR
        public Dictionary<int, int> eventListeners { get; private set; } = new Dictionary<int, int>();

        private void SetEditorData(int eventId , ListenerContainer[] containers)
        {
            if (containers == null)
            {
                return;
            }

            if (eventListeners.ContainsKey(eventId) == false)
            {
                eventListeners.Add(eventId, 0);
            }
            eventListeners[eventId]++;
        }

        public void RecordClear()
        {
            eventListeners.Clear();
        }

        public void GetEnum(ref Dictionary<int,string> m_enumString)
        {
            m_enumString.Clear();
            foreach (Type enumType in Assembly.GetExecutingAssembly().GetTypes()
                        .Where(x => x.IsSubclassOf(typeof(Enum)) && x.IsPublic))
            {
                try
                {
                    Array array = enumType.GetEnumValues();
                    for (int i = 0, Count = array.Length; i < Count; i++)
                    {
                        object o = array.GetValue(i);
                        int enumId = (int)o;
                        if (enumId > 100)
                        {
                            if (m_enumString.ContainsKey(enumId) == false)
                            {
                                m_enumString.Add(enumId, o.ToString());
                            }
                        }
                    }
                }
                catch (Exception e) { }
            }
        }
#endif
    }
}
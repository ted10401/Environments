using System;
using System.Collections;
using System.Collections.Generic;

namespace JSLCore.Coroutine
{
    public class CoroutineChain
    {
        private UnityEngine.Coroutine m_coroutine;
        private UnityEngine.Coroutine m_currentCoroutine;
        private Queue<IEnumerator> m_coroutineChains;

        public CoroutineChain()
        {
            m_coroutineChains = new Queue<IEnumerator>();
        }

        public CoroutineChain Enqueue(IEnumerator coroutine)
        {
            m_coroutineChains.Enqueue(coroutine);
            return this;
        }

        public CoroutineChain Enqueue(Action action)
        {
            m_coroutineChains.Enqueue(CreateEnumerator(action));
            return this;
        }

        public CoroutineChain Enqueue<T>(Action<T> action, T data)
        {
            m_coroutineChains.Enqueue(CreateEnumerator(action, data));
            return this;
        }

        private IEnumerator CreateEnumerator(Action action)
        {
            yield return null;
            action?.Invoke();
        }

        private IEnumerator CreateEnumerator<T>(Action<T> action, T data)
        {
            yield return null;
            action?.Invoke(data);
        }

        public CoroutineChain StartCoroutine()
        {
            m_coroutine = CoroutineManager.Instance.StartCoroutine(RunCoroutines());
            return this;
        }

        private IEnumerator RunCoroutines()
        {
            while (m_coroutineChains.Count > 0)
            {
                m_currentCoroutine = CoroutineManager.Instance.StartCoroutine(m_coroutineChains.Dequeue());
                yield return m_currentCoroutine;
            }
            
            StopCoroutine();
        }

        public void StopCoroutine()
        {
            m_coroutineChains.Clear();

            if(CoroutineManager.Instance != null)
            {
                return;
            }

            if(m_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(m_coroutine);
                m_coroutine = null;
            }

            if (m_currentCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(m_currentCoroutine);
                m_currentCoroutine = null;
            }
        }
    }
}
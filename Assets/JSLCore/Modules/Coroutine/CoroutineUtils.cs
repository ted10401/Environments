using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace JSLCore.Coroutine
{
    public static class CoroutineUtils
    {
        private const int PRECISION = 100;
        private const float CONST = 1f;

        private static WaitForEndOfFrame m_cacheWaitForEndOfFrame = new WaitForEndOfFrame();
        private static Dictionary<int, WaitForSeconds> m_cacheWaitForSeconds = new Dictionary<int, WaitForSeconds>();
        private static Dictionary<int, WaitForSecondsRealtime> m_cacheWaitForSecondsRealtime = new Dictionary<int, WaitForSecondsRealtime>();
        private static int m_cacheKey;
        private static float m_cacheSeconds;

        public static IEnumerator WaitForSeconds(float seconds)
        {
            m_cacheKey = (int)(seconds * PRECISION);

            if (!m_cacheWaitForSeconds.ContainsKey(m_cacheKey))
            {
                m_cacheSeconds = CONST / PRECISION * m_cacheKey;
                m_cacheWaitForSeconds.Add(m_cacheKey, new UnityEngine.WaitForSeconds(m_cacheSeconds));
            }

            yield return m_cacheWaitForSeconds[m_cacheKey];
        }

        public static IEnumerator WaitForSecondsRealtime(float seconds)
        {
            m_cacheKey = (int)(seconds * PRECISION);

            if (!m_cacheWaitForSecondsRealtime.ContainsKey(m_cacheKey))
            {
                m_cacheSeconds = CONST / PRECISION * m_cacheKey;
                m_cacheWaitForSecondsRealtime.Add(m_cacheKey, new UnityEngine.WaitForSecondsRealtime(m_cacheSeconds));
            }

            yield return m_cacheWaitForSecondsRealtime[m_cacheKey];
        }

        public static IEnumerator WaitForEndOfFrame()
        {
            yield return m_cacheWaitForEndOfFrame;
        }

        public static IEnumerator WaitUntil(Func<bool> predicate)
        {
            yield return new UnityEngine.WaitUntil(predicate);
        }

        public static IEnumerator WaitWhile(Func<bool> predicate)
        {
            yield return new UnityEngine.WaitWhile(predicate);
        }

        public static IEnumerator WaitForAnimation(Animator animator)
        {
            return WaitForAnimation(animator, 0);
        }

        public static IEnumerator WaitForAnimation(Animator animator, int layerIndex)
        {
            return WaitForSeconds(animator.GetCurrentAnimatorStateInfo(layerIndex).length);
        }
    }
}
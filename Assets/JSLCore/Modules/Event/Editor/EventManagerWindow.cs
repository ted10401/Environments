using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;
using System.Reflection;
using System.Linq;

namespace JSLCore.Event
{
    public class EventManagerWindow : EditorWindow
    {
        [MenuItem("JSLCore/EventManager Listener")]
        public static void OpenWindow()
        {
            EventManagerWindow window = GetWindow<EventManagerWindow>();
            window.titleContent = new GUIContent("事件發送次數監聽視窗");
            window.Show();
        }

        private EventManager m_eventManager;
        private Vector2 m_cacheScrollPos = Vector2.zero;
        private bool isReamlTimeRecording = false;
        private Dictionary<int, string> m_enumString = new Dictionary<int, string>();
        private Dictionary<int, int> m_eventListenerCount = null;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            
        }

        void Update()
        {
            if (isReamlTimeRecording)
            {
                if (EditorApplication.isPlaying && !EditorApplication.isPaused)
                {
                    Repaint();
                }
            }
        }
        private void OnGUI()
        {
            if (Application.isPlaying == false)
            {
                EditorGUILayout.LabelField("執行遊戲時，才會進行監聽");
                return;
            }
            else
            {
                if (m_eventManager == null)
                {
                    m_eventManager = EventManager.Instance;
                }
            }

            if (m_eventManager == null)
            {
                EditorGUILayout.LabelField("找不到 EventManager.");
                return;
            }

            isReamlTimeRecording = EditorGUILayout.Toggle("即時更新", isReamlTimeRecording);
            if (GUILayout.Button("清除次數"))
            {
                m_eventManager.RecordClear();
                return;
            }
            if (GUILayout.Button("轉換 enumId 為 英文"))
            {
                m_eventManager.GetEnum(ref m_enumString);
                return;
            }

            EditorGUILayout.BeginVertical();
            UpdateListenerCount();
            m_cacheScrollPos = EditorGUILayout.BeginScrollView(m_cacheScrollPos, GUILayout.ExpandHeight(true));

            string tempValue = "";
            foreach (int eventID in m_eventListenerCount.Keys)
            {
                EditorGUILayout.BeginHorizontal(); if (m_enumString.TryGetValue(eventID, out tempValue) == false)
                {
                    tempValue = eventID.ToString();
                }
                EditorGUILayout.LabelField(tempValue);
                EditorGUILayout.LabelField(string.Format("呼叫次數：{0}" , m_eventListenerCount[eventID]));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void UpdateListenerCount()
        {
            m_eventListenerCount = m_eventManager.eventListeners;
            string tempValue = "";
            foreach (int eventID in m_eventListenerCount.Keys)
            {
                if (m_enumString.TryGetValue(eventID, out tempValue) == false)
                {
                    tempValue = eventID.ToString();
                }
            }
        }
    }
}
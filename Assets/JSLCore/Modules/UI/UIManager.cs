using UnityEngine;
using System;
using JSLCore.Resource;
using System.Collections.Generic;
using System.IO;
using UnityEngine.EventSystems;

namespace JSLCore.UI
{
    public class UIManager : Singleton<UIManager>
    {
        private const string UI_CAMERA_TAG = "UICamera";
        private const string SHOW_UI_LAYER = "UI";
        private const string HIDE_UI_LAYER = "HideUI";

        private LayerMask m_showUILayer;
        private LayerMask m_hideUILayer;
        private Canvas m_cacheCanvas;

        public Camera camera { get; private set; }
        
        private Dictionary<string, UIComponent> m_loadedUIComponents;
        private string m_cacheKey;
        private UIComponent m_cacheUIComponent;

        public void Initialize()
        {
            m_showUILayer = LayerMask.NameToLayer(SHOW_UI_LAYER);
            m_hideUILayer = LayerMask.NameToLayer(HIDE_UI_LAYER);
            m_loadedUIComponents = new Dictionary<string, UIComponent>();

            InitializeUICamera();
            InitializeEventSystem();
        }

        private void InitializeUICamera()
        {
            GameObject cameraObj = GameObject.FindGameObjectWithTag(UI_CAMERA_TAG);
            if (cameraObj != null)
            {
                camera = cameraObj.GetComponent<Camera>();
            }
            else
            {
                cameraObj = new GameObject("UICamera");
                cameraObj.tag = "UICamera";
                cameraObj.gameObject.layer = LayerMask.NameToLayer(SHOW_UI_LAYER);

                camera = cameraObj.AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.Depth;
                camera.cullingMask = LayerMask.GetMask(SHOW_UI_LAYER);
                camera.orthographic = true;
                camera.orthographicSize = 5;
                camera.nearClipPlane = 0.3f;
                camera.farClipPlane = 1000f;
            }
        }

        private void InitializeEventSystem()
        {
            if (!GameObject.FindObjectOfType<EventSystem>())
            {
                GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem));
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        private void AddCache(string key, UIComponent uIComponent)
        {
            if(m_loadedUIComponents.ContainsKey(key))
            {
                m_loadedUIComponents[key] = uIComponent;
            }
            else
            {
                m_loadedUIComponents.Add(key, uIComponent);
            }

            SetActive(uIComponent, false);
        }

        public void Preload(string assetName, Action callback)
        {
            Preload(string.Empty, assetName, callback);
        }

        public void Preload(string assetBundleName, string assetName, Action callback)
        {
            m_cacheKey = Path.Combine(assetBundleName, assetName);
            m_loadedUIComponents.TryGetValue(m_cacheKey, out m_cacheUIComponent);

            if(m_cacheUIComponent == null)
            {
                ResourceManager.Instance.LoadAsync<UIComponent>(assetBundleName, assetName, delegate (UIComponent uIComponent)
                {
                    m_cacheUIComponent = GameObject.Instantiate(uIComponent, null);
                    m_cacheUIComponent.SetCanvas(camera);
                    AddCache(m_cacheKey, m_cacheUIComponent);

                    callback?.Invoke();
                });
            }
            else
            {
                callback?.Invoke();
            }
        }

        public void LoadUIAsync<T>(string assetName, Action<T> callback) where T : UnityEngine.Object
        {
            LoadUIAsync<T>(string.Empty, assetName, callback);
        }

        public void LoadUIAsync<T>(string assetBundleName, string assetName, Action<T> callback) where T : UnityEngine.Object
        {
            m_cacheKey = Path.Combine(assetBundleName, assetName);
            m_loadedUIComponents.TryGetValue(m_cacheKey, out m_cacheUIComponent);

            if (m_cacheUIComponent == null)
            {
                ResourceManager.Instance.LoadAsync<UIComponent>(assetBundleName, assetName, delegate (UIComponent uIComponent)
                {
                    m_cacheUIComponent = GameObject.Instantiate(uIComponent, null);
                    m_cacheUIComponent.SetCanvas(camera);
                    AddCache(m_cacheKey, m_cacheUIComponent);

                    callback?.Invoke(m_loadedUIComponents[m_cacheKey].GetComponent<T>());
                });
            }
            else
            {
                callback?.Invoke(m_loadedUIComponents[m_cacheKey].GetComponent<T>());
            }
        }

        public void LoadUIAsync<T>(string assetName, Action<T, object> callback, object customData) where T : UnityEngine.Object
        {
            LoadUIAsync<T>(string.Empty, assetName, callback, customData);
        }

        public void LoadUIAsync<T>(string assetBundleName, string assetName, Action<T, object> callback, object customData) where T : UnityEngine.Object
        {
            m_cacheKey = Path.Combine(assetBundleName, assetName);
            m_loadedUIComponents.TryGetValue(m_cacheKey, out m_cacheUIComponent);

            if(m_cacheUIComponent == null)
            {
                ResourceManager.Instance.LoadAsync<UIComponent>(assetBundleName, assetName, delegate (UIComponent uIComponent)
                {
                    m_cacheUIComponent = GameObject.Instantiate(uIComponent, null);
                    m_cacheUIComponent.SetCanvas(camera);
                    AddCache(m_cacheKey, m_cacheUIComponent);

                    callback?.Invoke(m_loadedUIComponents[m_cacheKey].GetComponent<T>(), customData);
                });
            }
            else
            {
                callback?.Invoke(m_loadedUIComponents[m_cacheKey].GetComponent<T>(), customData);
            }
        }

        private List<string> m_destroyViews = new List<string>();
        public void Destroy(UIComponent uIComponent)
        {
            if(uIComponent == null)
            {
                return;
            }

            uIComponent.Destroy();
            GameObject.Destroy(uIComponent.gameObject);
            m_destroyViews.Clear();
            foreach (KeyValuePair<string, UIComponent> kvp in m_loadedUIComponents)
            {
                if(kvp.Value == null)
                {
                    m_destroyViews.Add(kvp.Key);
                }
            }

            for(int i = 0, count = m_destroyViews.Count; i < count; i++)
            {
                m_loadedUIComponents.Remove(m_destroyViews[i]);
            }
        }

        public void SetActive(string assetName, bool active)
        {
            SetActive(string.Empty, assetName, active);
        }

        public void SetActive(string assetBundleName, string assetName, bool active)
        {
            m_cacheKey = Path.Combine(assetBundleName, assetName);
            m_loadedUIComponents.TryGetValue(m_cacheKey, out m_cacheUIComponent);
            SetActive(m_cacheUIComponent, active);
        }

        public void SetActive(UIComponent uIComponent, bool active)
        {
            if (uIComponent == null)
            {
                return;
            }

            uIComponent.SetActiveInternal(active);
        }
    }
}
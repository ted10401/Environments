#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace JSLCore.Utils
{
    public class ComponentUtils
    {
        [MenuItem("JSLCore/ComponentUtils/Cleanup Missing Scripts in the Active Scene")]
        public static void CleanupMissingScriptsInTheActiveScene()
        {
            GameObject[] rootGameObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();
            
            if (rootGameObjects == null || rootGameObjects.Length == 0)
            {
                JSLDebug.Log("[ComponentUtils] - There is no gameObjects.");
                return;
            }

            Object[] result = EditorUtility.CollectDeepHierarchy(rootGameObjects);
            int count = 0;

            foreach (Object obj in result)
            {
                count++;

                if(obj == null)
                {
                    continue;
                }

                if(EditorUtility.DisplayCancelableProgressBar("Hold on...", string.Format("Cleanup Missing Scripts in object '{0}'...{1}/{2}", obj.name, count, result.Length), (float)count / result.Length))
                {
                    break;
                }

                if(obj is GameObject gameObject)
                {
                    RemoveMissingScript(gameObject);
                }
            }

            EditorUtility.ClearProgressBar();

            EditorSceneManager.SaveOpenScenes();
        }

        private static void RemoveMissingScript(GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();
            if(components == null || components.Length == 0)
            {
                return;
            }

            SerializedObject serializedObject = new SerializedObject(gameObject);
            SerializedProperty serializedProperty = serializedObject.FindProperty("m_Component");

            for (int i = components.Length - 1; i >= 0; i--)
            {
                if (components[i] == null)
                {
                    JSLDebug.LogFormat("There is missing component in gameObject '{0}', destroy it.", gameObject.name);
                    serializedProperty.DeleteArrayElementAtIndex(i);

                    EditorSceneManager.MarkAllScenesDirty();
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
using UnityEngine;
using UnityEditor;

namespace JSLCore.Utils
{
    public abstract class StringEnumPropertyDrawer<T> : PropertyDrawer where T : struct, System.IConvertible
    {
        private SerializedProperty m_enumStringProperty;

        private string[] m_names;
        private int m_index = 0;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            Initialize(property);

            EditorGUI.BeginProperty(rect, label, property);

            m_names = GetEnumList();
            m_index = EditorGUI.Popup(rect, label.text, m_index, m_names);
            m_enumStringProperty.stringValue = m_names[m_index];

            EditorGUI.EndProperty();
        }

        private void Initialize(SerializedProperty property)
        {
            if (m_enumStringProperty != null)
            {
                return;
            }

            m_enumStringProperty = property.FindPropertyRelative("m_enumString");
            var eventName = m_enumStringProperty.stringValue;

            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            m_names = GetEnumList();
            for (int i = 0; i < m_names.Length; i++)
            {
                if (m_names[i] == eventName)
                {
                    m_index = i;
                    break;
                }
            }
        }

        public string[] GetEnumList()
        {
            return System.Enum.GetNames(typeof(T));
        }
    }
}

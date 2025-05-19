
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagNameAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �ˬd���ݩʬO�_�� string ����
        if (property.propertyType == SerializedPropertyType.String)
        {
            // ����Ҧ� Tag
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            // �d���e�Ȧb Tag �C��������
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(tags, property.stringValue));

            // �b Inspector ����ܬ��U�Կ��
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, tags);

            // ��s�ݩʭȬ���ܪ� Tag
            property.stringValue = tags[selectedIndex];



        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.HelpBox(position, $"{nameof(TagNameAttribute)} �u��Ω� string �����ݩ�", MessageType.Warning);
        }
    }
}

#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // �ˬd���ݩʬO�_�� string ����
        if (property.propertyType == SerializedPropertyType.String)
        {
            // ����Ҧ� Scene
            var scenes = EditorBuildSettings.scenes;
            string[] sceneNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            }

            // �d���e�Ȧb Scene �C��������
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(sceneNames, property.stringValue));

            // �b Inspector ����ܬ��U�Կ��
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames);

            // ��s�ݩʭȬ���ܪ� Scene
            property.stringValue = sceneNames[selectedIndex];



        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.HelpBox(position, $"{nameof(SceneNameAttribute)} �u��Ω� string �����ݩ�", MessageType.Warning);
        }
    }
}

#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 檢查該屬性是否為 string 類型
        if (property.propertyType == SerializedPropertyType.String)
        {
            // 獲取所有 Scene
            var scenes = EditorBuildSettings.scenes;
            string[] sceneNames = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            }

            // 查找當前值在 Scene 列表中的索引
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(sceneNames, property.stringValue));

            // 在 Inspector 中顯示為下拉選單
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneNames);

            // 更新屬性值為選擇的 Scene
            property.stringValue = sceneNames[selectedIndex];



        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.HelpBox(position, $"{nameof(SceneNameAttribute)} 只能用於 string 類型屬性", MessageType.Warning);
        }
    }
}

#endif

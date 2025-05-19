
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagNameAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 檢查該屬性是否為 string 類型
        if (property.propertyType == SerializedPropertyType.String)
        {
            // 獲取所有 Tag
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            // 查找當前值在 Tag 列表中的索引
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(tags, property.stringValue));

            // 在 Inspector 中顯示為下拉選單
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, tags);

            // 更新屬性值為選擇的 Tag
            property.stringValue = tags[selectedIndex];



        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.HelpBox(position, $"{nameof(TagNameAttribute)} 只能用於 string 類型屬性", MessageType.Warning);
        }
    }
}

#endif

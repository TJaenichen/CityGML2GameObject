using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// From https://answers.unity.com/questions/1005277/can-i-change-variable-name-on-inspector.html
/// </summary>
public class LabelOverride : PropertyAttribute
{
    public string label;
    public LabelOverride(string label)
    {
        this.label = label;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelOverride))]
    public class ThisPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyAttribute = this.attribute as LabelOverride;
            label.text = propertyAttribute.label;
            EditorGUI.PropertyField(position, property, label);
        }
    }
#endif
}
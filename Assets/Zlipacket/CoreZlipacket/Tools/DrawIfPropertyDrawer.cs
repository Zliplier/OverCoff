using UnityEditor;
using UnityEngine;

namespace Zlipacket.CoreZlipacket.Tools
{
    [CustomPropertyDrawer(typeof(DrawIfAttribute))]
    public class DrawIfPropertyDrawer : PropertyDrawer
    {
        // Reference to the attribute on the property
        DrawIfAttribute drawIf;
        // Field that is being compared
        SerializedProperty comparedField;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShowMe(property))
            {
                // If the condition is not met, hide the field by returning zero height
                return 0;
            }
            // Otherwise, show the field with the standard height
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // If the condition is met, draw the field
            if (ShowMe(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private bool ShowMe(SerializedProperty property)
        {
            drawIf = attribute as DrawIfAttribute;
            // Find the property that is being compared
            comparedField = property.serializedObject.FindProperty(drawIf.comparedPropertyName);

            if (comparedField == null)
            {
                Debug.LogError("Cannot find property: " + drawIf.comparedPropertyName);
                return true; // Default to showing the field if something is wrong
            }

            // Get the value of the compared field and check the condition
            // This example only works for boolean fields
            return comparedField.boolValue;
        }
    }
}
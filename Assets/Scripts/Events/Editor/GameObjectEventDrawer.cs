using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GameObjectEvent))]
public class GameObjectEventDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "Hello World");
        EditorGUI.PropertyField(position, property);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property, label);
	}
}

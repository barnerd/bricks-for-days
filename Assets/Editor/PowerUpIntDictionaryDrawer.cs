using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// ---------------
//  PowerUp => Int
// ---------------

[CustomEditor(typeof(PowerUpIntDictionary))]
public class PowerUpIntDictionaryDrawer : SerializableDictionaryDrawer<PowerUp, int>
{
    protected override SerializableKeyValueTemplate<PowerUp, int> GetTemplate()
    {
        return GetGenericTemplate<SerializablePowerUpIntTemplate>();
    }
}
internal class SerializablePowerUpIntTemplate : SerializableKeyValueTemplate<PowerUp, int> { }

/*   private WeightedPowerUps weightedListToDisplay
    {
        get
        {
            return target as WeightedPowerUps;
        }
    }

    private void OnEnable()
    {
        if (weightedListToDisplay.objects == null)
        {
            weightedListToDisplay.objects = new Dictionary<PowerUp, int>();
        }
    }

    private void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        // Column Headers
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("PowerUps: ");
        GUILayout.Label("Weights: ");
        GUILayout.EndHorizontal();

        foreach (var kvp in weightedListToDisplay.objects)
        {
            GUILayout.BeginHorizontal();
            //weightedListToDisplay = (PowerUp)EditorGUILayout.ObjectField(kvp.Key, typeof(PowerUp), false);
            //weight = EditorGUILayout.IntField(kvp.Value);
            EditorGUI.PropertyField(new Rect(50, 50, 50, 50), serializedObject.FindProperty("weight"), true);
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
        {
            PowerUp PU = ScriptableObject.CreateInstance<PowerUp>();
            weightedListToDisplay.objects.Add(PU, 0);
        }
        GUILayout.EndVertical();

        // this isn't quite working. need to use Property field & serialized objects/fields
        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(weightedListToDisplay);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
*/

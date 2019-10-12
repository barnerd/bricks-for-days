using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// ---------------
//  PowerUp => Int
// ---------------

[UnityEditor.CustomPropertyDrawer(typeof(GameObjectIntDictionary))]
public class GameObjectIntDictionaryDrawer : SerializableDictionaryDrawer<GameObject, int>
{
    protected override SerializableKeyValueTemplate<GameObject, int> GetTemplate()
    {
        return GetGenericTemplate<SerializablePowerUpIntTemplate>();
    }
}
internal class SerializablePowerUpIntTemplate : SerializableKeyValueTemplate<GameObject, int> { }

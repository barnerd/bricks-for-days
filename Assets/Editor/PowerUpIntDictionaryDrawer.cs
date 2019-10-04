using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

// ---------------
//  PowerUp => Int
// ---------------

[UnityEditor.CustomPropertyDrawer(typeof(PowerUpIntDictionary))]
public class PowerUpIntDictionaryDrawer : SerializableDictionaryDrawer<PowerUp, int>
{
    protected override SerializableKeyValueTemplate<PowerUp, int> GetTemplate()
    {
        return GetGenericTemplate<SerializablePowerUpIntTemplate>();
    }
}
internal class SerializablePowerUpIntTemplate : SerializableKeyValueTemplate<PowerUp, int> { }

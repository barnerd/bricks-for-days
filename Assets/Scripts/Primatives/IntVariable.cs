using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Primatives/IntVariable")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int InitialValue;
    [SerializeField] private bool IsConstant = true;
    [SerializeField] private string Description;

    private int _value;

    public int Value
    {
        get { return IsConstant ? InitialValue : _value; }
        set { _value = value; }
    }

    public void OnAfterDeserialize()
    {
        _value = InitialValue;
    }

    public void OnBeforeSerialize() { }
}

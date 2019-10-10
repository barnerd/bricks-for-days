using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StringVariable", menuName = "Primatives/StringVariable")]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private string InitialValue;
    [SerializeField] private bool IsConstant = true;
    [SerializeField] private string Description;

    private string _value;

    public string Value
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

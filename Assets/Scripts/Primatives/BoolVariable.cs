﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Primatives/BoolVariable")]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private bool InitialValue;
    [SerializeField] private bool IsConstant = true;
    [SerializeField] private string Description;

    private bool _value;

    public bool Value
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

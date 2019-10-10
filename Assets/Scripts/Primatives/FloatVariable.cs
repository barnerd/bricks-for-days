using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "Primatives/FloatVariable")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private float InitialValue;
    [SerializeField] private bool IsConstant = true;
    [SerializeField] private string Description;

    private float _value;

    public float Value
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

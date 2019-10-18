using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Add Int")]
public class PowerUpAddInt : PowerUp
{
    public int _value;
    public int minValue;
    public int maxValue;
    public IntReference _intSetting;

    public override void UsePowerUpPayload()
    {
        _intSetting.Value = Mathf.Clamp(_intSetting.Value + _value, minValue, maxValue);

        base.UsePowerUpPayload();
    }
}

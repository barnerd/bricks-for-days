using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Set Int")]
public class PowerUpSetInt : PowerUp
{
    public int _value;
    public IntReference _intSetting;

    public override void UsePowerUpPayload()
    {
        _intSetting.Value = Mathf.Max(_value, _intSetting.Value);

        base.UsePowerUpPayload();
    }
}

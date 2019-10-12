using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Add Int")]
public class PowerUpAddInt : PowerUp
{
    public int _value;
    public IntReference _intSetting;

    public override void UsePowerUpPayload()
    {
        _intSetting.Value += _value;

        base.UsePowerUpPayload();
    }
}

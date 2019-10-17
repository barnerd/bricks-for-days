using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Multiply Float")]
public class PowerUpMultiplyFloat : PowerUp
{
    public float _value;
    public FloatReference _floatSetting;

    public override void UsePowerUpPayload()
    {
        _floatSetting.Value *= _value;

        base.UsePowerUpPayload();
    }
}

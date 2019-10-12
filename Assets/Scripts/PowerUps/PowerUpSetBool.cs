using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Set Bool")]
public class PowerUpSetBool : PowerUp
{
    public BoolReference _booleanSetting;

    public override void UsePowerUpPayload()
    {
        _booleanSetting.Value = true;

        base.UsePowerUpPayload();
    }
}

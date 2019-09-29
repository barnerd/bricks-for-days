using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Ball Speed")]
public class PowerUpBallSpeed : PowerUp
{
    public float multiplier = 1;
    public FloatVariable ballSpeedMultiplier;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        ballSpeedMultiplier.Value = multiplier;
    }
}

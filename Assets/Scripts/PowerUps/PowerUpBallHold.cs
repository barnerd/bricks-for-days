using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Ball Hold")]
public class PowerUpBallHold : PowerUp
{
    public BoolVariable ballAlwaysHeld;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        ballAlwaysHeld.Value = true;
    }
}

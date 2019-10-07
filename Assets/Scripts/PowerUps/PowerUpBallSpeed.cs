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

        /* logic
         * current + powerup -> result
         * 2 + 2 -> 2 = do nothing
         * 2 + .5 -> 1
         * 1 + 2 -> 2
         * 1 + .5 -> .5
         * .5 + 2 -> 1
         * .5 + .5 -> .5 = do nothing
         * */
        if (Mathf.Approximately(ballSpeedMultiplier.Value, 1f))
        {
            ballSpeedMultiplier.Value = multiplier;
        }
        else if((ballSpeedMultiplier.Value > 1f && multiplier < 1f) || (ballSpeedMultiplier.Value < 1f && multiplier > 1f))
        {
            ballSpeedMultiplier = 1f;
        }
    }
}

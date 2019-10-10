using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Score Multiplier")]
public class PowerUpScoreMultiplier : PowerUp
{
    public int multipler = 1;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        scoreMultiplier.Value = Mathf.Max(multipler, scoreMultiplier.Value);
    }
}

using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Extra Lives")]
public class PowerUpExtraLives : PowerUp
{
    public int extraLives = 1;
    public IntVariable playerLives;

    public GameEvent OnPowerUpExtraLivesGain;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        playerLives.Value += extraLives;
        OnPowerUpExtraLivesGain.Raise();
    }
}

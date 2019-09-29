using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Evil Banana")]
public class PowerUpEvilBanana : PowerUp
{
    public int extraLives = 1;
    public IntVariable playerLives;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        playerLives.Value += extraLives;
    }
}

using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Ball Power")]
public class PowerUpBallPower : PowerUp
{
    public IntVariable ballPower;
    public GameEvent OnPowerBall;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        ballPower.Value = 99;
        OnPowerBall.Raise();
    }
}

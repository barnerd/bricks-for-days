using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Ball Power")]
public class PowerUpBallPower : PowerUp
{
    public IntVariable ballPower;
    public GameEvent OnPowerBall;
    public int increasedPower;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        ballPower.Value = increasedPower;
        OnPowerBall.Raise();
    }
}

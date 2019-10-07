using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Power Up/Evil Banana")]
public class PowerUpEvilBanana : PowerUp
{
    public BoolVariable bananaBall;
    public GameEvent OnBananaBall;

    public override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to change Sprite
        bananaBall.Value = true;
        OnBananaBall.Raise();
    }
}

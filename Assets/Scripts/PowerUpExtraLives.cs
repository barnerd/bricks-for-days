using UnityEngine;
using System.Collections;

public class PowerUpExtraLives : PowerUp
{
    public int extraLives = 1;
    public IntVariable playerLives;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -200f));
        score = 1000;
    }

    protected override void UsePowerUpPayload()
    {
        base.UsePowerUpPayload();

        // Payload is to add lives
        playerLives.Value += extraLives;
        Debug.Log("Extra Life added");
    }
}

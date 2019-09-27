using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Sprite powerupSprite;

    public static GameController gc;
    public static GameObject paddle;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("paddle"))
        {
            UsePowerUpPayload();
        }
    }

    protected virtual void UsePowerUpPayload()
    {
        // spawn a cool effect

        // apply power up
        paddle.transform.localScale *= 2f; // or .5f

        Destroy(gameObject);
    }
}

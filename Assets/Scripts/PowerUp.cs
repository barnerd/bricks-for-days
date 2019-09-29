using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int score;
    public Sprite powerUpSprite;

    public IntVariable gameScore;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -200f));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("paddle"))
        {
            Debug.Log("Collided with the paddle");
            UsePowerUpPayload();
        }
    }

    protected virtual void UsePowerUpPayload()
    {
        // spawn a cool effect

        // add score for collecting PowerUp
        gameScore.Value += score;

        // apply power up
        //paddle.transform.localScale *= 2f; // or .5f

        Destroy(gameObject);
    }
}

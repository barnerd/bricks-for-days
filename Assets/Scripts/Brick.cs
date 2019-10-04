using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level = 1;
    public int score = 1;
    public int scoreMultiplierPerLevel = 1;
    private int totalScore;
    public IntVariable gameScore;

    public GameEvent onLevelComplete;

    [Header("Sprites")]
    public List<Sprite> brickSprites;

    [Header("PowerUps")]
    public GameObject powerUp;

    public void SetLevel(int _level)
    {
        level = (_level >= 1 && _level <= 7) ? _level : 1;
        score = level * scoreMultiplierPerLevel;

        totalScore = 0;
        for (int i = 0; i < level; i++)
        {
            totalScore += scoreMultiplierPerLevel;
        }


        // update graphics of brick
        GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
    }

    public void DecreaseLevel(int decrease = 1)
    {
        level -= decrease;

        if (level <= 0)
        {
            gameScore.Value += totalScore;

            if (powerUp != null)
            {
                Instantiate(powerUp, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);

            // Set to 1 left as Destroy does not immediately completed.
            if (GameObject.FindGameObjectsWithTag("brick").Length <= 1)
            {
                onLevelComplete.Raise();
            }
        }
        else
        {
            gameScore.Value += score;
            totalScore -= score;
            score -= scoreMultiplierPerLevel;

            // display new brick level
            GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ball"))
        {
            DecreaseLevel(collision.collider.GetComponent<Ball>().BallPower.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            DecreaseLevel(collision.GetComponent<Ball>().BallPower.Value);
        }
    }

    public void TurnOnTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void ResetTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level = 1;
    public int score = 1;
    public int scoreMultiplierPerLevel = 1;
    private int totalScore;
    public IntReference gameScore;
    public IntReference scoreMultiplier;

    public GameEvent onLevelComplete;

    [Header("Sprites")]
    public List<Sprite> brickSprites;
    public Sprite brickBarrierBreakable;
    public Sprite brickBarrierUnbreakable;

    [Header("PowerUps")]
    public GameObject powerUp;

    void Start()
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_RandomOffset", Random.Range(0f, 5f));
    }

    public void SetLevel(int _level)
    {
        level = (_level >= 1 && _level <= 7 || _level == 63 || _level == 127) ? _level : 1;
        score = level * scoreMultiplierPerLevel;

        totalScore = 0;
        for (int i = 0; i < level; i++)
        {
            totalScore += scoreMultiplierPerLevel;
        }

        // update graphics of brick
        if(level <= 7)
        {
            GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        }
        else if (level == 63)
        {
            GetComponent<SpriteRenderer>().sprite = brickBarrierBreakable;
        }
        else if (level == 127)
        {
            GetComponent<SpriteRenderer>().sprite = brickBarrierUnbreakable;
        }
    }

    public void DecreaseLevel(int decrease = 1)
    {
        if(level == 63 || level == 127)
        {
            if(decrease >= level)
            {
                gameScore.Value += score * scoreMultiplier.Value;

                Destroy(gameObject);
            }
        }
        else
        {
            level -= decrease;

            if (level <= 0)
            {
                gameScore.Value += totalScore * scoreMultiplier.Value;

                if (powerUp != null)
                {
                    Instantiate(powerUp, transform.position, Quaternion.identity);
                }

                Destroy(gameObject);

                // TODO: Count only non-barriers
                // Set to 1 left as Destroy does not immediately completed.
                if (GameObject.FindGameObjectsWithTag("brick").Length <= 1)
                {
                    onLevelComplete.Raise();
                }
            }
            else
            {
                gameScore.Value += score * scoreMultiplier.Value;
                totalScore -= score;
                score -= scoreMultiplierPerLevel;

                // display new brick level
                GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ball"))
        {
            DecreaseLevel(collision.collider.GetComponent<Ball>().ballPower.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            DecreaseLevel(collision.GetComponent<Ball>().ballPower.Value);
        }
    }

    public void SetTrigger()
    {
        GetComponent<Collider2D>().isTrigger |= level != 127;
    }
}
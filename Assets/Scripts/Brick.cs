﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level = 1;
    public int score = 1;
    public bool hasPowerUp = false;
    public IntVariable gameScore;

    public GameEvent onLevelComplete;

    [Header("Sprites")]
    public List<Sprite> brickSprites;

    [Header("PowerUps")]
    public GameObject powerUpPrefab;

    public void initBrick(int _level, bool _hasPowerUp)
    {
        level = (_level >= 1 && _level <= 7) ? _level : 1;
        score = level;

        hasPowerUp = _hasPowerUp;

        // update graphics of brick
        GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
    }

    public void decreaseLevel(int decrease = 1)
    {
        level -= decrease;

        int totalScore = 0;
        for (int i = 0; i < decrease && score > 0; i++)
        {
            totalScore += score;
            //Debug.Log("sscore: " + score);
            //Debug.Log("step: " + totalScore);
            score -= 1;
        }
        gameScore.Value += totalScore;

        if (level <= 0)
        {
            // figure out why this is 1 and there's one left over
            // Debug.Log(GameObject.FindGameObjectsWithTag("brick").Length);
            if (GameObject.FindGameObjectsWithTag("brick").Length <= 1)
            {
                onLevelComplete.Raise();
            }
            else if (hasPowerUp)
            {
                Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else
        {
            // display new brick level
            GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ball"))
        {
            decreaseLevel(collision.collider.GetComponent<Ball>().BallPower.Value);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ball"))
        {
            decreaseLevel(collision.GetComponent<Ball>().BallPower.Value);
        }
    }

    public void TurnToTrigger()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void ResetCollider()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
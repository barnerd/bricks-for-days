using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level = 1;
    public int score = 10;
    public bool hasPowerUp = false;
    public IntVariable gameScore;

    [Header("Sprites")]
    public List<Sprite> brickSprites;

    public void initBrick(int _level, bool _hasPowerUp)
    {
        level = (_level >= 1 && _level <= 7) ? _level : 1;
        score = level;

        hasPowerUp = _hasPowerUp;

        // update graphics of brick
        GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        Debug.Log(brickSprites[level - 1]);
    }

    public void decreaseLevel()
    {
        level -= 1;

        gameScore.Value += score;
        score -= 1;

        if (level <= 0)
        {
            if (hasPowerUp)
            {
                GameObject prefab = Resources.Load("Prefabs/HalfBallSpeedPowerUp") as GameObject;
                Debug.Log(prefab + " created");

                Instantiate(prefab, transform.position, Quaternion.identity);
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
        if (collision.collider.tag == "ball")
        {
            decreaseLevel();
        }
    }
}
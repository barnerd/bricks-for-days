using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level;
    public int score;

    private GameController gc;

    [Header("Sprites")]
    public List<Sprite> brickSprites;

    public void initBrick(int l, GameController g)
    {
        level = (l >= 1 && l <= 7) ? l : 1;
        score = 10 * level;

        gc = g;

        // update graphics of brick
        GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        Debug.Log(brickSprites[level - 1]);
    }

    public void decreaseLevel()
    {
        level -= 1;

        if (level <= 0)
        {
            gc.Score(score);
            Destroy(gameObject);

            // TODO: Figure out why there's one left over
            if (GameObject.FindGameObjectsWithTag("brick").Length == 1)
            {
                gc.LevelWon();
            }
        }
        else
        {
            gc.Score(score);
            score -= 10;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level = 1;
    public int score = 10;
    public bool hasPowerUp = false;

    private GameController gc;

    [Header("Sprites")]
    public List<Sprite> brickSprites;

    public void initBrick(int _level, bool _hasPowerUp, GameController _gameController)
    {
        level = (_level >= 1 && _level <= 7) ? _level : 1;
        score = 10 * level;

        hasPowerUp = _hasPowerUp;

        gc = _gameController;

        // update graphics of brick
        GetComponent<SpriteRenderer>().sprite = brickSprites[level - 1];
        Debug.Log(brickSprites[level - 1]);
    }

    public void decreaseLevel()
    {
        level -= 1;

        gc.Score(score);
        score -= 10;

        if (level <= 0)
        {
            if (hasPowerUp)
            {
                //Instantiate(PowerUp, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);

            // TODO: Figure out why there's one left over
            if (GameObject.FindGameObjectsWithTag("brick").Length == 1)
            {
                gc.LevelWon();
            }
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

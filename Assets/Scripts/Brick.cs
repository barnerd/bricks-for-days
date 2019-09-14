using UnityEngine;

public class Brick : MonoBehaviour
{
    public int level;
    public int score;

    private GameController gc;

    public void initBrick(int l, GameController g)
    {
        level = 1;
        score = 10 * level;

        gc = g;

        // update graphics of brick
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
            // display new brick level
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

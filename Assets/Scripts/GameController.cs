using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game Values")]
    public int score;
    public int lives;

    public bool gameHasEnded = false;

    [Space]

    [Header("UI Components")]
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    void Start()
    {
        gameHasEnded = false;
    }

    public void Score(int s)
    {
        score += s;
    }

    public void LevelWon()
    {
        completeLevelUI.SetActive(true);
    }

    public void LoseLife()
    {
        lives -= 1;

        if (lives == 0)
        {
            EndGame();
        }
        else
        {
            // reset level
            FindObjectOfType<BallController>().ResetBall();
        }
    }

    public void EndGame()
    {
        // show end game UI
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            gameOverUI.SetActive(true);
        }
    }
}

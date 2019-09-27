using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game Values")]
    public int score;
    private int _lives;
    public int lives { get { return _lives; } }

    public bool gameHasEnded = false;

    [Space]

    [Header("UI Components")]
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    void Start()
    {
        gameHasEnded = false;
    }

    public void Score(int _score)
    {
        score += _score;
    }

    public void LevelWon()
    {
        completeLevelUI.SetActive(true);
    }

    public void GainLife(int _life = 1)
    {
        if (_life >= 0)
        {
            _lives += _life;
        }
        else
        {
            LoseLife(_life);
        }

    }

    public void LoseLife(int _life = 1)
    {
        if(_life >= 0)
        {
            _lives -= _life;

            if (_lives == 0)
            {
                EndGame();
            }
            else
            {
                // reset level
                FindObjectOfType<BallController>().ResetBall();
            }
        }
        else
        {
            GainLife(_life);
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public IntVariable gameScore;
    public IntVariable scoreMultiplier;
    public IntVariable playerLives;

    [Header("UI Components")]
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    private void Start()
    {
        StartGame();
        Time.timeScale = 4f;
    }

    public void StartGame()
    {
        gameScore.Value = 0;
        ResetScoreMultiplier();
        playerLives.Value = 5;
        Time.timeScale = 1f;
    }

    public void LevelWon()
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        // show end game UI
        gameOverUI.SetActive(true);
    }

    public void ResetScoreMultiplier()
    {
        scoreMultiplier.Value = 1;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public IntVariable gameScore;
    public IntVariable playerLives;

    public float gameScale;

    [Header("UI Components")]
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    private void Start()
    {
        StartGame();
        Time.timeScale = gameScale;
    }

    // TODO: Figure out who is Raising() onGameStart event
    public void StartGame()
    {
        gameScore.Value = 0;
        playerLives.Value = 5;
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
}

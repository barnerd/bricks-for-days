using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Values")]
    public IntReference gameScore;
    public IntReference playerLives;

    public bool gameHasEnded = false;

    [Space]

    [Header("UI Components")]
    public GameObject completeLevelUI;
    public GameObject gameOverUI;

    void Start()
    {
        gameHasEnded = false;
    }

    void Update()
    {
        if (playerLives.Value == 0)
        {
            gameHasEnded = true;
            EndGame();
        }
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

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public IntVariable gameScore;
    public IntVariable scoreMultiplier;
    public IntVariable playerLives;
    public float AITimeScale;
    public KeyCode pauseKey;
    public KeyCode optionsKey;

    [Header("UI Components")]
    public GameObject pauseUI;
    public GameObject levelCompleteUI;
    public GameObject gameOverUI;
    public GameObject highscoresUI;
    public GameObject optionsUI;
    public GameObject powerupInfoUI;
    public GameObject creditsUI;

    private Highscores hs;
    private int newHighscoreRank;
    private GameObject nameInput;

    private void Start()
    {
        ResetGameValues();
        ResetScoreMultiplier();
        playerLives.Value = 5;

        Time.timeScale = AITimeScale;

        // Add function call to wantsToQuit event
        Application.wantsToQuit += Exiting;
        hs = new Highscores();
        hs.Load();

        // Update UI with highscores
        UpdateHighScoresUI();
    }

    private void Update()
    {
        if (Input.GetKeyUp(pauseKey))
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            Time.timeScale = (!pauseUI.activeSelf) ? 0f : 1f;
        }
        if (Input.GetKeyUp(optionsKey) && !optionsUI.activeSelf)
        {
            OpenWindow(optionsUI);
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        ResetGameValues();
        ResetScoreMultiplier();
        Time.timeScale = 1f;
    }

    // TODO: move to UI Manager?
    public void LevelWon()
    {
        OpenWindow(levelCompleteUI);
    }

    public void EndGame()
    {
        newHighscoreRank = hs.AddHighscore(gameScore.Value);

        if (newHighscoreRank >= 0)
        {
            // There's a new highscore
            UpdateHighScoresUI();
            OpenWindow(highscoresUI);
        }
        // show end game UI
        OpenWindow(gameOverUI);
    }

    // TODO: move to UI Manager
    public void SaveHighscoresName(InputField _text)
    {
        if (newHighscoreRank >= 0 && _text.text != null && _text.text != "")
        {
            hs.names[newHighscoreRank] = _text.text;
            newHighscoreRank = -1;

            UpdateHighScoresUI();
        }
    }

    public void ResetGameValues()
    {
        gameScore.Value = 0;
        playerLives.Value = 5;
        newHighscoreRank = -1;
    }

    public void ResetScoreMultiplier()
    {
        scoreMultiplier.Value = 1;
    }

    // TODO: move to UI Manager
    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }

    // TODO: move to UI Manager
    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);

        if (!pauseUI.activeSelf &&
        !levelCompleteUI.activeSelf &&
        !gameOverUI.activeSelf &&
        !highscoresUI.activeSelf &&
        !optionsUI.activeSelf)
        /* TODO add these in once they're created
         * &&
    !powerupInfoUI.activeSelf &&
    !creditsUI)*/
        {
            Time.timeScale = 1f;
        }
    }

    private bool Exiting()
    {
        hs.Save();
        return true;
    }

    // TODO: move to UI Manager
    public void ResetHighScores()
    {
        hs = new Highscores();
        UpdateHighScoresUI();
    }

    // TODO: move to UI Manager
    public void UpdateHighScoresUI()
    {
        Transform bg = highscoresUI.transform.Find("Background");
        Transform scoreEntry;
        for (int i = 0; i < 10; i++)
        {
            scoreEntry = bg.Find("Score" + (i + 1));

            if (hs.names[i] == null)
            {
                scoreEntry.gameObject.SetActive(false);
            }
            else
            {
                scoreEntry.gameObject.SetActive(true);
                scoreEntry.Find("Player").gameObject.SetActive(newHighscoreRank != i);
                scoreEntry.Find("Player").GetComponent<Text>().text = hs.names[i];
                scoreEntry.Find("PlayerInputField").gameObject.SetActive(newHighscoreRank == i);
                scoreEntry.Find("Date").GetComponent<Text>().text = hs.dates[i].ToString("MM/dd/yy");
                scoreEntry.Find("Score").GetComponent<Text>().text = hs.scores[i].ToString("N0");
            }
        }
    }
}

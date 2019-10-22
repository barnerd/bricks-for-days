using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public IntReference gameScore;
    public IntReference scoreMultiplier;
    public IntReference playerLives;
    public float AITimeScale;
    public KeyCode pauseKey;
    public GameEvent OnLevelStart;
    private Camera camera;

    [Header("UI Components")]
    public GameObject pauseUI;
    public GameObject levelCompleteUI;
    public GameObject gameOverUI;
    public GameObject highscoresUI;

    private Highscores hs;
    private int newHighscoreRank;
    private GameObject nameInput;

    [Header("Audio Components")]
    public AudioMixer soundMixer;

    [Header("Game States")]
    public BoolReference gameOver;

    public GameEvent onGameStart;

    private void Start()
    {
        camera = Camera.main;
        GenerateCollidersAcrossScreen();

        Time.timeScale = AITimeScale;

        ResetGameValues();
        ResetScoreMultiplier();

        if (highscoresUI != null)
        {
            // Add function call to wantsToQuit event
            Application.wantsToQuit += Exiting;
            hs = new Highscores();
            hs.Load();

            // Update UI with highscores
            UpdateHighScoresUI();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(pauseKey))
        {
            if (pauseUI != null)
            {
                pauseUI.GetComponent<ScreenController>().SetWindowState(!Mathf.Approximately(Time.timeScale, 0f));
            }

            Time.timeScale = (!Mathf.Approximately(Time.timeScale, 0f)) ? 0f : 1f;
        }
    }

    void GenerateCollidersAcrossScreen()
    {
        Vector2 bottomLeftCorner = camera.ViewportToWorldPoint(new Vector3(0, 0f, camera.nearClipPlane));
        Vector2 topRightCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        float height = topRightCorner.y - bottomLeftCorner.y;
        float width = topRightCorner.x - bottomLeftCorner.x;
        Vector2[] colliderPoints;

        EdgeCollider2D upperEdge = transform.Find("topWall").GetComponent<EdgeCollider2D>();
        colliderPoints = upperEdge.points;
        colliderPoints[0] = new Vector2(bottomLeftCorner.x, 0);
        colliderPoints[1] = new Vector2(topRightCorner.x, 0);
        upperEdge.points = colliderPoints;
        upperEdge.offset = new Vector2(0, topRightCorner.y - height * .08f); // move topWall 8% below top of screen

        EdgeCollider2D lowerEdge = transform.Find("bottomWall").GetComponent<EdgeCollider2D>();
        colliderPoints = lowerEdge.points;
        colliderPoints[0] = new Vector2(bottomLeftCorner.x, 0);
        colliderPoints[1] = new Vector2(topRightCorner.x, 0);
        lowerEdge.points = colliderPoints;
        lowerEdge.offset = new Vector2(0, bottomLeftCorner.y - height * .1f); // move bottomWall 10% below screen

        EdgeCollider2D leftEdge = transform.Find("leftWall").GetComponent<EdgeCollider2D>();
        colliderPoints = leftEdge.points;
        colliderPoints[0] = new Vector2(0, topRightCorner.y);
        colliderPoints[1] = new Vector2(0, bottomLeftCorner.y - height * .1f);
        leftEdge.points = colliderPoints;
        leftEdge.offset = new Vector2(bottomLeftCorner.x + width * .09f, 0); // move leftWall in 9%

        EdgeCollider2D rightEdge = transform.Find("rightWall").GetComponent<EdgeCollider2D>();
        colliderPoints = rightEdge.points;
        colliderPoints[0] = new Vector2(0, topRightCorner.y);
        colliderPoints[1] = new Vector2(0, bottomLeftCorner.y - height * .1f);
        rightEdge.points = colliderPoints;
        rightEdge.offset = new Vector2(topRightCorner.x - width * .09f, 0); // move rightWall in 9%
    }

    public void StartGame()
    {
        ResetGameValues();
        ResetScoreMultiplier();
        ResumePlay();
        gameOver.Value = false;
        StartCoroutine(StartNewLevel(.5f));
    }

    // TODO: move to UI Manager?
    public void LevelWon()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.GetComponent<ScreenController>().StartNotificationFade();
        }
        StartCoroutine(StartNewLevel(2f));
    }

    public void EndGame()
    {
        gameOver.Value = true;

        newHighscoreRank = hs.AddHighscore(gameScore.Value);

        if (newHighscoreRank >= 0)
        {
            // There's a new highscore
            UpdateHighScoresUI();
            highscoresUI.GetComponent<ScreenController>().SetWindowState(true);
        }
        else
        {
            StartCoroutine(StartNewGame());
        }
    }

    public void OnUIWindowClose(MonoBehaviour obj)
    {
        if (gameOver.Value && obj.gameObject == highscoresUI)
        {
            StartCoroutine(StartNewGame());
        }
    }

    public IEnumerator StartNewGame()
    {
        gameOverUI.GetComponent<ScreenController>().StartNotificationFade();
        yield return new WaitForSeconds(3f);
        onGameStart.Raise();
    }

    public IEnumerator StartNewLevel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        OnLevelStart.Raise();
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
        gameScore.Value = gameScore.Variable.InitialValue;
        playerLives.Value = playerLives.Variable.InitialValue;
        newHighscoreRank = -1;
    }

    public void ResetScoreMultiplier()
    {
        scoreMultiplier.Value = scoreMultiplier.Variable.InitialValue;
    }

    public void PausePlay()
    {
        Time.timeScale = 0f;
    }

    public void ResumePlay()
    {
        Time.timeScale = 1f;
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
        Transform bg = highscoresUI.transform.Find("Background").Find("High Scores Table");
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

    // TODO: move to UI Manager
    public void ToggleMusic(bool _sound)
    {
        if (_sound)
        {
            soundMixer.ClearFloat("backgroundVolume");
        }
        else
        {
            soundMixer.SetFloat("backgroundVolume", -80f);
        }
    }

    // TODO: move to UI Manager
    public void ToggleSoundFX(bool _sound)
    {
        if (_sound)
        {
            soundMixer.ClearFloat("ballFXVolume");
            soundMixer.ClearFloat("soundFXVolume");
        }
        else
        {
            soundMixer.SetFloat("ballFXVolume", -80f);
            soundMixer.SetFloat("soundFXVolume", -80f);
        }
    }
}

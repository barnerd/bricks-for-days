using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    private bool isLoaded;

    private AsyncOperation async;
    public Canvas loadingScreen;
    public Text loadingText;
    public Slider loadingProgressBar;
    private float _loadingProgress;
    public float LoadingProgress { get { return _loadingProgress; } }

    public GameEvent onGameStart;

    private void Start()
    {
        isLoaded = false;

        StartCoroutine(LoadNewScene());
    }

    private void Update()
    {
        if (isLoaded && Input.anyKey)
        {
            // TODO: fade SceneLoader out
            loadingScreen.gameObject.SetActive(false);
            onGameStart.Raise();
            SceneManager.UnloadSceneAsync(0);
        }
    }

    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    public IEnumerator LoadNewScene()
    {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        async.allowSceneActivation = false;

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (async.progress < 0.9f)
        {
            _loadingProgress = Mathf.Clamp01(async.progress / 0.9f);
            loadingProgressBar.value = LoadingProgress;

            yield return null;
        }

        isLoaded = true;
        // TODO: fade progress bar out
        loadingProgressBar.gameObject.SetActive(false);
        // TODO: localize text
        loadingText.text = "Press any key";
        async.allowSceneActivation = true;
    }
}

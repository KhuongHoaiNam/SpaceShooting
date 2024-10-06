using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : SingletonMono<Loader>
{
    public float timeEndLoadGame = 4f;
    private float timeStartLoadGame = 0f;

    public SenceId senceId;

    [SerializeField] private GameObject LoadbarStartGame;
    [SerializeField] private Image imageLoadingStartGamer;
    [SerializeField] private GameObject LoaderBarOnGame;
    [SerializeField] private Image imgLoadingOnGame;

    private bool loadGame = true;
    private bool loaderOnGame = false;

    private void Start()
    {
        if (loadGame)
        {
            // Start the loading game process via Coroutine
            StartCoroutine(OnLoadStartGameCoroutine());
        }
    }

    // Coroutine for starting the game loading process
    private IEnumerator OnLoadStartGameCoroutine()
    {
        LoadbarStartGame.SetActive(true);

        // Simulating a loading process with time
        while (timeStartLoadGame <= timeEndLoadGame)
        {
            timeStartLoadGame += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(timeStartLoadGame / timeEndLoadGame);
            imageLoadingStartGamer.fillAmount = fillAmount;
            yield return null; // Wait for the next frame
        }

        // Once loading is complete
        imageLoadingStartGamer.fillAmount = 1f;
        LoadbarStartGame.SetActive(false);

        // Load the initial view after the loading completes
        LoadingView(SenceId.MennuView, ViewIndex.MennuView);
        loadGame = false;
    }

    // Reset the current scene
    public void ResetSence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Reset loader values
    public void ResetLoader()
    {
        timeStartLoadGame = 0;
        loaderOnGame = true;
    }

    // Load a view by scene and view index
    public void LoadingView(SenceId senceId, ViewIndex viewIndex)
    {
        ViewManager.Instance.viewindex = viewIndex;
        SceneManager.LoadScene(viewIndex.ToString());
    }

    // Load a game view with loading bar for in-game transition
    public void LoadGameSelectView(SenceId senceId, ViewIndex viewIndex)
    {
        loaderOnGame = true;
        if (loaderOnGame)
        {
            LoaderBarOnGame.SetActive(loaderOnGame);
            StartCoroutine(LoadingCoroutine(senceId, viewIndex));
        }
    }

    // Coroutine for loading scenes with a loading bar
    private IEnumerator LoadingCoroutine(SenceId senceId, ViewIndex viewIndex)
    {
        // Reset loading values
        timeStartLoadGame = 0f;

        // Async scene loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(senceId.ToString());

        // While the scene is loading
        while (!asyncLoad.isDone)
        {
            timeStartLoadGame += Time.deltaTime;
            float fillLoader = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            imgLoadingOnGame.fillAmount = fillLoader; // Update loader UI

            // Wait for the next frame
            yield return null;
        }

        // Once loading is complete
        loaderOnGame = false;
        timeStartLoadGame = 1f;
        LoaderBarOnGame.SetActive(false);

        // Load the new view and reset loader state
        ViewManager.SwitchView(viewIndex);
        ResetLoader();
    }

    // Load scene directly
    public void Loading(SenceId senceId)
    {
        ViewManager.Instance.CloseAllViews();
        SceneManager.LoadScene(senceId.ToString());
    }
}

using Project.Scripts;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class GameManager : MonoBehaviour
{
    public int scoreToWin;
    public int currentScore;

    public bool gamePaused;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        TryStopGame();
        TryTogglePauseGame();
    }

    public void TryTogglePauseGame()
    {
        if (!Input.GetButtonDown("Cancel")) return;
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        gamePaused = !gamePaused;
        FreezeGameIf(gamePaused);
        UnFreezeGameIf(!gamePaused);
        GameUI.Instance.TogglePauseMenu(gamePaused);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        GameUI.Instance.UpdateScoreText(score);
        if (currentScore >= scoreToWin)
        {
            WinGame();
        }
    }

    public void LoseGame()
    {
        GameUI.Instance.SetEndGameScreen(false, currentScore);

        // TODO: We may want to do this in a more coordinated fashion
        FreezeGame();
    }

    private static void TryStopGame()
    {
        if (!Input.GetButtonDown("EditorQuit")) return;

        Debug.Log("Stopping game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }


    private void WinGame()
    {
        GameUI.Instance.SetEndGameScreen(true, currentScore);

        // TODO: We may want to do this in a more coordinated fashion
        FreezeGame();
    }

    private void FreezeGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void UnFreezeGameIf(bool unfreeze)
    {
        if (!unfreeze) return;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FreezeGameIf(bool shouldFreeze)
    {
        if (shouldFreeze) FreezeGame();
    }
}

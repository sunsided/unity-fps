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
        Time.timeScale = gamePaused ? 0 : 1;
        Cursor.lockState = gamePaused ? CursorLockMode.None : CursorLockMode.Locked;

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


    private void WinGame() => GameUI.Instance.SetEndGameScreen(true, currentScore);
}

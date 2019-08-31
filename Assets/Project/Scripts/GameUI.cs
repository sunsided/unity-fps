using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class GameUI : MonoBehaviour
    {
        [Header("HUD")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI ammoText;
        public UnityEngine.UI.Image healthBarFill;

        [Header("Pause Menu")]
        public GameObject pauseMenu;

        [Header("End Game Screen")]
        public GameObject endGameScreen;
        public TextMeshProUGUI endGameHeaderText;
        public TextMeshProUGUI endGameScoreText;

        public static GameUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void UpdateHealthBar(int currentHp, int maxHp) => healthBarFill.fillAmount = (float) currentHp / maxHp;

        public void UpdateScoreText(int score) => scoreText.text = $"Score: {score}";

        public void UpdateAmmoText(int currentAmmo, int maxAmmo) => ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";

        public void TogglePauseMenu(bool paused) => pauseMenu.SetActive(paused);

        public void SetEndGameScreen(bool gameWon, int score)
        {
            endGameScreen.SetActive(true);
            endGameHeaderText.color = gameWon ? Color.green : Color.red;
            endGameHeaderText.text = gameWon ? "You Win!" : "You Lose!";
            endGameScoreText.text = $"<b>Score</b>\n{score}";
        }

        [UsedImplicitly]
        public void OnResumeButton() => GameManager.Instance.TryTogglePauseGame();

        [UsedImplicitly]
        public void OnRestartButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        [UsedImplicitly]
        public void OnMenuButton() => SceneManager.LoadScene("Menu");
    }
}

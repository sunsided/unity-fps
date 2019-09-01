using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts
{
    public class MenuUI : MonoBehaviour
    {
        public void OnPlayButton()
        {
            SceneManager.LoadScene("Game");
        }

        public void OnQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
        }
    }
}

using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Project.Scripts
{
    public class GameController : MonoBehaviour
    {
        void Update()
        {
            StopGame();
        }

        private static void StopGame()
        {
            if (!Input.GetButtonDown("Cancel")) return;

            Debug.Log("Stopping game.");

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

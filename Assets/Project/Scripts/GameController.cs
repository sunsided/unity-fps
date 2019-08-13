using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

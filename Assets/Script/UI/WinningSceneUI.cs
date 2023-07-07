using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningSceneUI : MonoBehaviour
{
    [SerializeField]private GameObject WinUIHolder;
    [SerializeField]private GameObject LeaderboardHolder;

    public void Continue()
    {
        WinUIHolder.SetActive(false);
        LeaderboardHolder.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("In-Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

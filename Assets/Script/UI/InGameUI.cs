using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField]private GameObject pauseScreenHolder;
    [SerializeField]private GameObject optionsHolder;
    public bool isPaused;

    public void Restart()
    {
        OptionsUI optionsUI = FindObjectOfType<OptionsUI>();
        if (optionsUI != null)
        {
            optionsUI.SaveVolumeSettings(); // Save volume settings before restarting
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenPauseMenu()
    {
        isPaused = true;
        pauseScreenHolder.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        isPaused = false;
        pauseScreenHolder.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenOptions()
    {
        pauseScreenHolder.SetActive(false);
        optionsHolder.SetActive(true);
    }

    public void CloseOptions()
    {
        pauseScreenHolder.SetActive(true);
        optionsHolder.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        OptionsUI optionsUI = FindObjectOfType<OptionsUI>();
        if (optionsUI != null)
        {
            optionsUI.SaveVolumeSettings(); // Save volume settings before returning to the main menu
        }

        Debug.Log("The In-Game Scene has been unloaded, and we are now in the MainMenu Scene");
        SceneManager.LoadScene(0);
    }
}

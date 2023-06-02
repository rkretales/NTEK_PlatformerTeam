using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUIHolder;
    [SerializeField] private GameObject OptionsUIHolder;
    
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OpenOptions()
    {
        MainMenuUIHolder.SetActive(false);
        OptionsUIHolder.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

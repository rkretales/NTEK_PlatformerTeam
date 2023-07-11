using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.Tools;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUIHolder;
    [SerializeField] private GameObject OptionsUIHolder;
    [SerializeField] private MMSoundManager _soundManager;

    public void Start()
    {
        _soundManager = FindObjectOfType(typeof(MMSoundManager)) as MMSoundManager;
        
    }

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
        Debug.Log("The game has exited");
    }
}

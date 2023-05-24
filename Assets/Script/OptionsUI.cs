using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionsUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject OptionsUIHolder;       
    public AudioMixer audioMixer;  
    public TMPro.TMP_Dropdown resolutionDropdown; 
    public TMPro.TMP_Dropdown graphicsQuality;
    Resolution[] resolutions;
    public float volumeValue;
    public int qualityIndexValue;
    public static bool bFullscreen;
    public Slider volumeSlider;

    void Start()
    {
        Screen.fullScreen = bFullscreen;

        Screen.SetResolution(PlayerPrefs.GetInt("resolutionWidth"), PlayerPrefs.GetInt("resolutionHeight"), Screen.fullScreen);

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityValue"));
        graphicsQuality.value = PlayerPrefs.GetInt("qualityValue");

        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");


        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OpenOptions()
    {
        MainMenuUI.SetActive(false);
        OptionsUIHolder.SetActive(true);
    }

    public void OptionReturn()
    {
        MainMenuUI.SetActive(true);
        OptionsUIHolder.SetActive(false);
    }

    public void SetVolume (float volume)
    {
        volumeValue = volume;
        audioMixer.SetFloat("MasterVolume", volumeValue);
        PlayerPrefs.SetFloat("MasterVolume", volumeValue); 
    }

    public void SetQuality (int qualityIndex)
    {
        PlayerPrefs.SetInt("qualityValue", qualityIndex); 
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityValue"));
    }

    public void SetFullscreen(bool isFullscreen)
    {
        bFullscreen = isFullscreen;
        Screen.fullScreen = bFullscreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(PlayerPrefs.GetInt("resolutionWidth"), PlayerPrefs.GetInt("resolutionHeight"), Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionWidth", resolution.width);
        PlayerPrefs.SetInt("resolutionHeight", resolution.height);
    }
}

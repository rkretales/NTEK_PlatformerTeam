using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Lowscope.Saving;

public class OptionsUI : MonoBehaviour {
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject OptionsUIHolder;
    public AudioMixer audioMixer;
    public TMP_Dropdown FPSDropdown;
    public TMP_Dropdown graphicsQuality;
    public float volumeValue;
    public Slider volumeSlider;

    private void Start() {
        Application.targetFrameRate = PlayerPrefs.GetInt("FPSLimit");
        //FPSDropdown.value = PlayerPrefs.GetInt("FPSLimitValue");

        QualitySettings.SetQualityLevel(SaveMaster.GetInt("qualityValue"));
        graphicsQuality.value = SaveMaster.GetInt("qualityValue");

        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
    }

    public void OptionReturn() {
        MainMenuUI.SetActive(true);
        OptionsUIHolder.SetActive(false);
    }

    public void SetVolume(float volume) {
        volumeValue = volume;
        audioMixer.SetFloat("MasterVolume", volumeValue);
        SaveMaster.SetFloat("MasterVolume", volumeValue);
        PlayerPrefs.SetFloat("MasterVolume", volumeValue);
    }

    public void SetQuality(int qualityIndex) {
        PlayerPrefs.SetInt("qualityValue", qualityIndex);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityValue"));
    }

    public void SetFPSLimit(int FPSIndex) {
        PlayerPrefs.SetInt("FPSLimitValue", FPSIndex);
        switch(FPSIndex) {
            case 0:
                PlayerPrefs.SetInt("FPSLimit", 30);
                Application.targetFrameRate = PlayerPrefs.GetInt("FPSLimit");
                break;

            case 1:
                PlayerPrefs.SetInt("FPSLimit", 60);
                Application.targetFrameRate = PlayerPrefs.GetInt("FPSLimit");
                break;
        }
    }
}
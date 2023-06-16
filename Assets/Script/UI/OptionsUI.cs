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
        Application.targetFrameRate = SaveMaster.GetInt("FPSLimit");
        FPSDropdown.value = SaveMaster.GetInt("FPSLimitValue");
        QualitySettings.SetQualityLevel(SaveMaster.GetInt("qualityValue"));
        graphicsQuality.value = SaveMaster.GetInt("qualityValue");

        volumeSlider.value = SaveMaster.GetFloat("MasterVolume");
    }

    public void OptionReturn() {
        MainMenuUI.SetActive(true);
        OptionsUIHolder.SetActive(false);
    }

    public void SetVolume(float volume) {
        volumeValue = volume;
        audioMixer.SetFloat("MasterVolume", volumeValue);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFPSLimit(int FPSIndex) {
        PlayerPrefs.SetInt("FPSLimitValue", FPSIndex);
        switch(FPSIndex) {
            case 0:
                Application.targetFrameRate = 30;
                break;

            case 1:
                Application.targetFrameRate = 60;
                break;
        }
    }
}
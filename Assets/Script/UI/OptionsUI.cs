using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Lowscope.Saving;
using UnityEngine.Serialization;

public class OptionsUI : MonoBehaviour {
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject OptionsUIHolder;
    public AudioMixer audioMixer;
    public TMP_Dropdown FPSDropdown;
    public TMP_Dropdown graphicsQuality;
    private float masterVolumeValue;
    private float sfxVolumeValue;
    private float musicVolumeValue;
    
    [Tooltip("The Volume sliders should be attached here from the New Options")]
    [Header("Volume Sliders")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    private void Start() {
        Application.targetFrameRate = SaveMaster.GetInt("FPSLimit");
        FPSDropdown.value = SaveMaster.GetInt("FPSLimitValue");
        QualitySettings.SetQualityLevel(SaveMaster.GetInt("qualityValue"));
        graphicsQuality.value = SaveMaster.GetInt("qualityValue");

        masterVolumeSlider.value = SaveMaster.GetFloat(key: "MasterVolume");
        sfxVolumeSlider.value = SaveMaster.GetFloat(key: "SfxVolume");
        musicVolumeSlider.value = SaveMaster.GetFloat(key: "MusicVolume");
        
    }

    public void OptionReturn() {
        MainMenuUI.SetActive(true);
        OptionsUIHolder.SetActive(false);
    }
#region VolumeSettings
    public void SetMasterVolume(float masterVolume)
    {
        masterVolumeValue = masterVolume;
        audioMixer.SetFloat("MasterVolume", masterVolumeValue);
    }
    
    public void SetSfxVolume(float sfxVolume)
    {
        sfxVolumeValue = sfxVolume;
        audioMixer.SetFloat("SfxVolume", sfxVolumeValue);
    }
    
    public void SetMusicVolume(float musicVolume)
    {
        musicVolumeValue = musicVolume;
        audioMixer.SetFloat("MusicVolume", musicVolumeValue);
    }
#endregion
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
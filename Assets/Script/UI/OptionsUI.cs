using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Lowscope.Saving;
using MoreMountains.Tools;
using UnityEngine.Serialization;

public class OptionsUI : MonoBehaviour {
    [SerializeField] private GameObject MainMenuUI;
    [SerializeField] private GameObject OptionsUIHolder;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown FPSDropdown;
    [SerializeField] private TMP_Dropdown graphicsQuality;
    
    [Tooltip("The Volume sliders should be attached here from the New Options")]
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private MMSoundManager _soundManager;

    private void Start() {
        Application.targetFrameRate = SaveMaster.GetInt("FPSLimit");
        FPSDropdown.value = SaveMaster.GetInt("FPSLimitValue");
        QualitySettings.SetQualityLevel(SaveMaster.GetInt("qualityValue"));
        graphicsQuality.value = SaveMaster.GetInt("qualityValue");

        masterVolumeSlider.value = SaveMaster.GetFloat(key: "MasterVolume");
        sfxVolumeSlider.value = SaveMaster.GetFloat(key: "SfxVolume");
        musicVolumeSlider.value = SaveMaster.GetFloat(key: "MusicVolume");

        _soundManager = FindObjectOfType<MMSoundManager>();
    }

    private void SaveVolumeSettings()
    {
        SaveMaster.SetFloat("MasterVolume", masterVolumeSlider.value);
        SaveMaster.SetFloat("SfxVolume", sfxVolumeSlider.value);
        SaveMaster.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void OptionReturn() {
        SaveVolumeSettings(); // Save volume settings before returning to the main menu
        MainMenuUI.SetActive(true);
        OptionsUIHolder.SetActive(false);
    }
#region VolumeSettings
    public void SetMasterVolume()
    {
        _soundManager.SetVolumeMaster(masterVolumeSlider.value);
        audioMixer.SetFloat("MasterVolume", masterVolumeSlider.value);
    }
    
    public void SetSfxVolume(float sfxVolume)
    {
        _soundManager.SetVolumeSfx(sfxVolumeSlider.value);
        audioMixer.SetFloat("SfxVolume", sfxVolumeSlider.value);
    }
    
    public void SetMusicVolume(float musicVolume)
    {
        _soundManager.SetVolumeMusic(musicVolumeSlider.value);
        audioMixer.SetFloat("MusicVolume", musicVolumeSlider.value);
    }
#endregion
    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFPSLimit(int fpsIndex) {
        PlayerPrefs.SetInt("FPSLimitValue", fpsIndex);
        Application.targetFrameRate = fpsIndex switch {
            0 => 30,
            
            1 => 60,
            
            _ => Application.targetFrameRate
        };
    }
}
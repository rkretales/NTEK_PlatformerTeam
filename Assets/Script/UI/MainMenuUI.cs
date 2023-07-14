using Lowscope.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MoreMountains.Tools;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuUIHolder;
    [SerializeField] private GameObject OptionsUIHolder;
    [HideInInspector][SerializeField] private MMSoundManager _soundManager;

    [Tooltip("The Volume sliders should be attached here from the New Options")]
    [Header("Volume Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    public void Start() // To initialize the volumes
    {
        _soundManager = FindObjectOfType(typeof(MMSoundManager)) as MMSoundManager;
        if (SaveMaster.GetString(key:"isFirstTime") == ""){SetDefaultVolume();}
    }

    private void SetDefaultVolume()
    {
        // To set the default volumes
        _soundManager.SetVolumeMaster(0.5f);
        _soundManager.SetVolumeSfx(1f);
        _soundManager.SetVolumeMusic(1f);
        
        // To show the default volumes on the sliders
        masterVolumeSlider.value = _soundManager.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, false);
        sfxVolumeSlider.value = _soundManager.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        musicVolumeSlider.value =_soundManager.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
        
        // To say that the game has been opened before
        SaveMaster.SetString(key:"isFirstTime", value:"true");
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

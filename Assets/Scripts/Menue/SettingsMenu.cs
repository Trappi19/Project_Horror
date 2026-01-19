using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization.SmartFormat.Utilities;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Mixers")]
    public AudioMixer masterVolume;
    public AudioMixer SFXVolume;
    public AudioMixer voicesVolume;
    public AudioMixer musicVolume;

    [Header("Slider Volumes")]
    public Slider masterVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider voicesVolumeSlider;
    public Slider musicVolumeSlider;

    void Start()
    {
         LoadVolumes();
    }
    public void SetVolumeMaster(float volume)
    {
        float masterVolumeValue = volume;
        masterVolume.SetFloat("masterVolume", volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetVolumeSFX(float volume)
    {
        float SFXVolumeValue = volume;
        SFXVolume.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetVolumeVoices(float volume)
    {
        float voicesVolumeValue = volume;
        voicesVolume.SetFloat("voicesVolume", volume);
        PlayerPrefs.SetFloat("voicesVolume", volume);
    }

    public void SetVolumeMusic(float volume)
    {
        float musicVolumeValue = volume;
        musicVolume.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
    }

    private void LoadVolumes()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("masterVolume", 0f);
        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume", 0f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);
        float savedVoicesVolume = PlayerPrefs.GetFloat("voicesVolume", 0f);

        masterVolume.SetFloat("masterVolume", savedMasterVolume);
        musicVolume.SetFloat("musicVolume", savedMusicVolume);
        SFXVolume.SetFloat("SFXVolume", savedSFXVolume);
        voicesVolume.SetFloat("voicesVolume", savedVoicesVolume);

        masterVolumeSlider.value = savedMasterVolume;
        SFXVolumeSlider.value = savedSFXVolume;
        musicVolumeSlider.value = savedMusicVolume;
        voicesVolumeSlider.value = savedVoicesVolume;
    }
}

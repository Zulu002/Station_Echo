using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    void Start()
    {
        // Громкость
        if (volumeSlider != null && audioMixer != null)
        {
            float volume = PlayerPrefs.GetFloat("Volume", 0.75f);
            volumeSlider.value = volume;
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }

        // Качество
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
            int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
            qualityDropdown.value = savedQuality;
            qualityDropdown.RefreshShownValue();
            QualitySettings.SetQualityLevel(savedQuality);
        }

        // Полноэкранный режим
        if (fullscreenToggle != null)
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }
    }

    // Метод для применения настроек
    public void ApplySettings()
    {
        SetVolume(volumeSlider.value);
        SetQuality(qualityDropdown.value);
        SetFullscreen(fullscreenToggle.isOn);
        Debug.Log("Настройки применены");
    }

    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("Volume", volume);
        }
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("Quality", index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}

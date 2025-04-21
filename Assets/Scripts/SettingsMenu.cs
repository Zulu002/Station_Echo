using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    void Awake()
    {
        // Устанавливаем разрешение и fullscreen из сохранений при запуске
        int width = PlayerPrefs.GetInt("ResolutionWidth", 1920);
        int height = PlayerPrefs.GetInt("ResolutionHeight", 1080);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.SetResolution(width, height, isFullscreen);
    }

    void Start()
    {
        // 🎚 Громкость
        if (volumeSlider != null && audioMixer != null)
        {
            float volume = PlayerPrefs.GetFloat("Volume", 0.75f);
            volumeSlider.value = volume;
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }

        // 🪟 Полноэкранный режим
        if (fullscreenToggle != null)
        {
            bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }

        // 📺 Разрешения
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            var options = new List<string>();
            HashSet<string> addedResolutions = new HashSet<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string resString = resolutions[i].width + " x " + resolutions[i].height;

                if (!addedResolutions.Contains(resString))
                {
                    addedResolutions.Add(resString);
                    filteredResolutions.Add(resolutions[i]);
                    options.Add(resString);

                    if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = filteredResolutions.Count - 1;
                    }
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void ApplySettings()
    {
        float volume = volumeSlider.value;
        bool fullscreen = fullscreenToggle.isOn;
        int resolutionIndex = resolutionDropdown.value;

        SetVolume(volume);
        SetResolution(resolutionIndex, fullscreen);

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);

        Resolution selectedRes = filteredResolutions[resolutionIndex];
        PlayerPrefs.SetInt("ResolutionWidth", selectedRes.width);
        PlayerPrefs.SetInt("ResolutionHeight", selectedRes.height);

        Debug.Log("Настройки применены");
    }

    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }
    }

    public void SetResolution(int resolutionIndex, bool fullscreen)
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0)
            return;

        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}

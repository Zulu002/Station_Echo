using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // 🎚 Громкость
        if (volumeSlider != null && audioMixer != null)
        {
            float volume = PlayerPrefs.GetFloat("Volume", 0.75f);
            volumeSlider.value = volume;
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }

        // 🖼️ Качество
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(QualitySettings.names));
            int savedQuality = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
            qualityDropdown.value = savedQuality;
            qualityDropdown.RefreshShownValue();
            QualitySettings.SetQualityLevel(savedQuality);
        }

        // 🪟 Fullscreen
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

            var options = new System.Collections.Generic.List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                if (!options.Contains(option))
                    options.Add(option);

                if (resolutions[i].width == 1920 && resolutions[i].height == 1080)
                {
                    currentResolutionIndex = i;
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
        int quality = qualityDropdown.value;
        bool fullscreen = fullscreenToggle.isOn;
        int resolutionIndex = resolutionDropdown.value;

        SetVolume(volume);
        SetQuality(quality);
        SetResolution(resolutionIndex, fullscreen);

        // Сохраняем только громкость, качество и фуллскрин
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("Quality", quality);
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);

        Debug.Log("Настройки применены");
    }

    public void SetVolume(float volume)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetResolution(int resolutionIndex, bool fullscreen)
    {
        if (resolutions == null || resolutions.Length == 0)
            return;

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;



    public void NewGame()
    {
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Level1");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            PlayerPrefs.SetInt("IsNewGame", 0); //  очень важно!
            PlayerPrefs.Save();
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("Нет сохранённого прогресса.");
        }
    }

    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Игра завершена");
    }
}

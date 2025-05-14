using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public UnityEngine.UI.Button continueButton; 

    private void Start()
    {
        
        if (continueButton != null)
        {
            continueButton.interactable = PlayerPrefs.HasKey("CheckpointX");
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("IsNewGame", 1);
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.Save();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.lastCheckpointPosition = GameManager.Instance.initialPosition;
        }


        SceneManager.LoadScene("Level1");
    }


    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            PlayerPrefs.SetInt("IsNewGame", 0);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("Нет сохранённого чекпоинта.");
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

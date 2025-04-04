using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 lastCheckpointPosition;
    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadCheckpoint(); // При запуске — загружаем чекпоинт
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartFromCheckpoint()
    {
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition;
        }
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        PlayerPrefs.SetFloat("CheckpointX", checkpointPosition.x);
        PlayerPrefs.SetFloat("CheckpointY", checkpointPosition.y);
        PlayerPrefs.SetFloat("CheckpointZ", checkpointPosition.z);
        PlayerPrefs.Save();
    }

    public void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            lastCheckpointPosition = new Vector3(
                PlayerPrefs.GetFloat("CheckpointX"),
                PlayerPrefs.GetFloat("CheckpointY"),
                PlayerPrefs.GetFloat("CheckpointZ")
            );
        }
    }

    public void RestartLevel() // если всё же нужно перезапустить сцену
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

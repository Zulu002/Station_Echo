using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 initialPosition; 
    public Vector3 lastCheckpointPosition; 
    public GameObject player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
        bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

        if (isNewGame)
        {
            lastCheckpointPosition = initialPosition;
            Debug.Log("Новая игра — старт с начальной позиции");
        }
        else
        {
            LoadCheckpoint();
            Debug.Log("Продолжение игры — загрузка чекпоинта");
        }

        
        PlayerPrefs.DeleteKey("IsNewGame");
        PlayerPrefs.Save();
    }

    private void Start()
    {
        
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition; 
            Debug.Log("Игрок перемещён на позицию: " + lastCheckpointPosition);
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetFloat("CheckpointZ", position.z);
        PlayerPrefs.Save();
        Debug.Log($"Checkpoint сохранён: {position}");
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            lastCheckpointPosition = new Vector3(x, y, z);
            Debug.Log($"Чекпоинт загружен: {lastCheckpointPosition}");
        }
        else
        {
            lastCheckpointPosition = initialPosition; 
            Debug.Log("Чекпоинт не найден — старт с начальной позиции");
        }
    }

    public Vector3 GetPlayerStartPosition()
    {
        return lastCheckpointPosition;
    }
}

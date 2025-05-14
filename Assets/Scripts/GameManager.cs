using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Vector3 initialPosition; // Начальная позиция
    public Vector3 lastCheckpointPosition; // Позиция последнего чекпоинта
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

        // Если это новая игра, то используем начальную позицию, если нет, то загрузим чекпоинт
        bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

        if (isNewGame)
        {
            lastCheckpointPosition = initialPosition; // В новой игре всегда начинаем с начальной позиции
            Debug.Log("Новая игра — старт с начальной позиции");
        }
        else
        {
            LoadCheckpoint(); // Загружаем чекпоинт, если он есть
            Debug.Log("Продолжение игры — загрузка чекпоинта");
        }

        // Удаляем флаг новой игры
        PlayerPrefs.DeleteKey("IsNewGame");
        PlayerPrefs.Save();
    }

    private void Start()
    {
        // Устанавливаем позицию игрока на начало
        if (player != null)
        {
            player.transform.position = lastCheckpointPosition; // Устанавливаем позицию в соответствии с последним чекпоинтом или начальной позицией
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
            lastCheckpointPosition = initialPosition; // Если чекпоинт не найден, начинаем с начальной позиции
            Debug.Log("Чекпоинт не найден — старт с начальной позиции");
        }
    }

    public Vector3 GetPlayerStartPosition()
    {
        return lastCheckpointPosition;
    }
}

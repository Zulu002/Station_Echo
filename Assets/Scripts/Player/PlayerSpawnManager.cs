using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    
    public GameObject playerOverride;

    private void Awake()
    {
        GameObject player = playerOverride != null ? playerOverride : GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Игрок не найден!");
            return;
        }

        
        GameManager.Instance.player = player;

        
        bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

        if (isNewGame)
        {
            player.transform.position = GameManager.Instance.initialPosition;
            Debug.Log("Новая игра — игрок на начальной позиции");
        }
        else
        {
            player.transform.position = GameManager.Instance.lastCheckpointPosition;
            Debug.Log("Продолжение — игрок на позиции последнего чекпоинта");
        }

        
        PlayerPrefs.DeleteKey("IsNewGame");
        PlayerPrefs.Save();
    }
}

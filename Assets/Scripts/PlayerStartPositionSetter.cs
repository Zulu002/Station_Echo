using UnityEngine;

public class PlayerStartPositionSetter : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameManager.Instance.player = player;

            bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

            if (isNewGame)
            {
                player.transform.position = GameManager.Instance.initialPosition;
                Debug.Log("Новая игра: спавн на начальной позиции");
            }
            else
            {
                player.transform.position = GameManager.Instance.lastCheckpointPosition;
                Debug.Log($"Продолжение игры: спавн с чекпоинта {GameManager.Instance.lastCheckpointPosition}");
            }
        }
        else
        {
            Debug.LogWarning("Игрок не найден!");
        }
    }


}

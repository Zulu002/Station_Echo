using UnityEngine;

public class RespawnAtCheckpoint : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.player = this.gameObject;

            
            bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

            if (isNewGame)
            {
                transform.position = GameManager.Instance.initialPosition;
                Debug.Log("Новая игра — игрок перемещён на стартовую позицию");
            }
            else
            {
                transform.position = GameManager.Instance.lastCheckpointPosition;
                Debug.Log("Продолжение — игрок перемещён в чекпоинт");
            }

            
            PlayerPrefs.DeleteKey("IsNewGame");
            PlayerPrefs.Save();
        }
    }
}

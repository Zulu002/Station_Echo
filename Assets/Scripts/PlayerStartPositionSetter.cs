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
                Debug.Log("����� ����: ����� �� ��������� �������");
            }
            else
            {
                player.transform.position = GameManager.Instance.lastCheckpointPosition;
                Debug.Log($"����������� ����: ����� � ��������� {GameManager.Instance.lastCheckpointPosition}");
            }
        }
        else
        {
            Debug.LogWarning("����� �� ������!");
        }
    }


}

using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    
    public GameObject playerOverride;

    private void Awake()
    {
        GameObject player = playerOverride != null ? playerOverride : GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("����� �� ������!");
            return;
        }

        
        GameManager.Instance.player = player;

        
        bool isNewGame = PlayerPrefs.GetInt("IsNewGame", 0) == 1;

        if (isNewGame)
        {
            player.transform.position = GameManager.Instance.initialPosition;
            Debug.Log("����� ���� � ����� �� ��������� �������");
        }
        else
        {
            player.transform.position = GameManager.Instance.lastCheckpointPosition;
            Debug.Log("����������� � ����� �� ������� ���������� ���������");
        }

        
        PlayerPrefs.DeleteKey("IsNewGame");
        PlayerPrefs.Save();
    }
}

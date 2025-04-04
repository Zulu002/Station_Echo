using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint saved!");
        }
    }
}

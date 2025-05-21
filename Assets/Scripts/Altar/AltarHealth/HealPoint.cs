using UnityEngine;
using UnityEngine.UI;

public class HealPoint : MonoBehaviour
{
    public GameObject interactionUI; 
    public AudioClip healSound;      
    private AudioSource audioSource;

    private bool playerInRange = false;
    private PlayerHealth playerHealth;

    private void Start()
    {
        interactionUI.SetActive(false); 
        audioSource = GetComponent<AudioSource>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<PlayerHealth>();
            interactionUI.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerHealth != null)
            {
                playerHealth.RestoreAllLives(); 

                if (healSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(healSound); 
                }
            }
        }
    }
}

using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door; 
    private Animator animator;
    private AudioSource audioSource; 
    public AudioClip buttonSound; 
    private bool isActivated = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && (collision.CompareTag("Player") || collision.CompareTag("MovableObject")))
        {
            isActivated = true;
            animator.SetBool("isPressed", true);

            // Воспроизведение звука
            if (buttonSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(buttonSound);
            }

            if (door != null)
                door.SetDoorState(true);
        }
    }
}

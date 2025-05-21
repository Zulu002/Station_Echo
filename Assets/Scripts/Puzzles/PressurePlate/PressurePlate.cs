using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door; 
    public AudioClip pressSound; 
    private AudioSource audioSource; 

    private Animator animator;
    private int objectsOnPlate = 0;
    private bool isPressed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnPlate++;
            UpdatePlateState();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            objectsOnPlate--;
            UpdatePlateState();
        }
    }

    private void UpdatePlateState()
    {
        bool newPressed = objectsOnPlate > 0;

        if (newPressed != isPressed)
        {
            isPressed = newPressed;

            if (animator != null)
                animator.SetBool("Pressed", isPressed);

            if (door != null)
                door.GetComponent<Door>().SetDoorState(isPressed);

            
            if (isPressed && audioSource != null && pressSound != null)
                audioSource.PlayOneShot(pressSound);
        }
    }
}

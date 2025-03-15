using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject door; // —сылка на дверь
    private Animator animator;
    private int objectsOnPlate = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        bool isPressed = objectsOnPlate > 0;

        if (animator != null)
        {
            animator.SetBool("Pressed", isPressed);
        }

        if (door != null)
        {
            door.GetComponent<Door>().SetDoorState(isPressed);
        }
    }
}

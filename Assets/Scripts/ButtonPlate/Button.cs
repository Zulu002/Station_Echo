using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door; // Ссылка на дверь
    private Animator animator; // Аниматор кнопки
    private bool isActivated = false; // Флаг, чтобы сработало только один раз

    private void Start()
    {
        animator = GetComponent<Animator>(); // Получаем аниматор кнопки
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && (collision.CompareTag("Player") || collision.CompareTag("MovableObject")))
        {
            isActivated = true; // Запоминаем, что кнопку уже нажали
            animator.SetBool("isPressed", true); // Запускаем анимацию
            if (door != null)
                door.SetDoorState(true); // Открываем дверь
        }
    }
}

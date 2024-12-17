using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 5f; // Скорость подъема по лестнице
    private Rigidbody2D rb;
    private bool isClimbing = false; // Флаг для проверки нахождения на лестнице
    private float originalGravity; // Для сохранения изначальной гравитации
    public Animator animator; // Аниматор для анимации подъема

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale; // Сохраняем исходное значение гравитации
    }

    void Update()
    {
        // Если игрок находится на лестнице, обрабатываем подъем
        if (isClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical"); // Получаем ввод по вертикали

            // Если игрок двигается вверх или вниз
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);

            // Отключаем гравитацию, чтобы не падать
            rb.gravityScale = 0;

            // Проигрываем анимацию подъема
            if (animator != null)
            {
                //animator.SetBool("Climbing", vertical != 0);
            }
        }
        else
        {
            // Восстанавливаем гравитацию, если не на лестнице
            rb.gravityScale = originalGravity;

            // Проигрываем анимацию "не поднимается"
            if (animator != null)
            {
                //animator.SetBool("Climbing", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, попал ли игрок на объект с тегом "Ladder"
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Выходим из состояния подъема, если покидаем лестницу
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}

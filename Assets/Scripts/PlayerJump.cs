using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 10f; // Сила прыжка
    public Transform groundCheck; // Точка проверки земли
    public LayerMask groundLayer; // Слой земли
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Проверяем, находится ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Обновляем параметр анимации Jumping в аниматоре
        animator.SetBool("Jumping", !isGrounded); // Прыжок: когда не на земле, анимация включена

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Прыжок: устанавливаем скорость по оси Y для выполнения прыжка
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // Добавляем звук или эффекты, если нужно (например, прыжковый эффект)
        }
    }
}

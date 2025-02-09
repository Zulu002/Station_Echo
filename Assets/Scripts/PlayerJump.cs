using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 10f; // Сила прыжка
    public Transform groundCheck; // Точка проверки земли
    public LayerMask groundLayer; // Слой земли
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator animator;

    // Настройки для кайоттайм
    public float coyoteTime = 0.2f; // Время, в течение которого можно прыгать после потери контакта с землей
    private float coyoteTimeCounter; // Таймер кайоттайм

    // Настройки для количества прыжков
    public int maxAirJumps = 1; // Максимальное количество прыжков в воздухе
    private int remainingJumps; // Остаток прыжков

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

        // Логика кайоттайм
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Если персонаж на земле, обновляем таймер
            remainingJumps = maxAirJumps; // Сбрасываем оставшиеся прыжки
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Уменьшаем таймер, если персонаж в воздухе
        }

        // Прыжок
        if (Input.GetButtonDown("Jump"))
        {
            // Прыжок возможен, если:
            // - Остался время на кайоттайм (т.е. персонаж был на земле недавно)
            // - Остались прыжки в воздухе
            if (coyoteTimeCounter > 0f || remainingJumps > 0)
            {
                // Выполняем прыжок
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // Логика прыжков в воздухе с учетом кайоттайма
                if (!isGrounded)
                {
                    if (coyoteTimeCounter > 0f)
                    {
                        // Если мы находимся в кайоттайме, даем еще один дополнительный прыжок
                        remainingJumps = maxAirJumps;
                    }
                    else if (remainingJumps > 0)
                    {
                        remainingJumps--; // Обычный прыжок в воздухе
                    }
                }

                // Обнуляем таймер кайоттайм, чтобы избежать двойного прыжка с земли
                coyoteTimeCounter = 0f;
            }
        }
    }
}

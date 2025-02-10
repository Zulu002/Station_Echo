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
    [Header("Настройки для кайот-тайм")]
    public float coyoteTime = 0.2f; // Время, в течение которого можно прыгать после потери контакта с землей
    private float coyoteTimeCounter; // Таймер кайоттайм

    
    [Header("Настройки для количества прыжков")]
    public int maxAirJumps = 1; // Максимальное количество прыжков в воздухе
    private int remainingJumps; // Остаток прыжков

    
    [Header("Настройки для динамического прыжка")]
    public float jumpTimeMax = 0.3f;       // Максимальное время удержания прыжка
    public float jumpCutMultiplier = 3f;   // Усиление гравитации при раннем отпускании
    private bool isJumping;
    private float jumpTimeCounter;

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

        // Начало прыжка
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || remainingJumps > 0)
            {
                StartJump();
            }
        }

        // Продолжение прыжка (долгое удержание)
        if (Input.GetButton("Jump") && isJumping)
        {
            ContinueJump();
        }

        // Преждевременное окончание прыжка
        if (Input.GetButtonUp("Jump"))
        {
            EndJumpEarly();
        }
    }

    private void StartJump()
    {
        isJumping = true;
        jumpTimeCounter = jumpTimeMax;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (!isGrounded)
        {
            if (coyoteTimeCounter > 0f)
            {
                remainingJumps = maxAirJumps;
            }
            else if (remainingJumps > 0)
            {
                remainingJumps--;
            }
        }

        coyoteTimeCounter = 0f; // Сброс таймера кайоттайм
    }

    private void ContinueJump()
    {
        if (jumpTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private void EndJumpEarly()
    {
        isJumping = false;

        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / jumpCutMultiplier);
        }
    }
}

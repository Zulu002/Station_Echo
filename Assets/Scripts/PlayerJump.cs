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
    public float jumpTimeMax = 0.3f; // Максимальное время удержания прыжка
    public float jumpCutMultiplier = 3f; // Усиление гравитации при раннем отпускании
    private bool isJumping;
    private float jumpTimeCounter;

    private HungerSystem hungerSystem; // Добавляем ссылку на HungerSystem
    private bool isJumpingUp = false; // Новый флаг для прыжка

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hungerSystem = GetComponent<HungerSystem>(); // Получаем компонент HungerSystem
    }

    void Update()
    {
        // Проверяем, находится ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Обновляем параметр анимации Jumping (но только если именно прыжок, а не падение)
        animator.SetBool("Jumping", isJumpingUp);

        // Обновляем анимацию падения (когда падаем вниз)
        animator.SetBool("Falling", !isGrounded && rb.linearVelocity.y < 0);

        // Если персонаж на земле, сбрасываем флаг прыжка
        if (isGrounded)
        {
            isJumpingUp = false;
            coyoteTimeCounter = coyoteTime;
            remainingJumps = maxAirJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Начало прыжка
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f || remainingJumps > 0)
            {
                if (isGrounded || coyoteTimeCounter > 0f)
                {
                    StartJump();
                }
                else if (hungerSystem != null && hungerSystem.CanJump())
                {
                    StartJump();
                    hungerSystem.OnJump();
                }
            }
        }

        // Продолжение прыжка
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
        isJumpingUp = true; // Устанавливаем флаг прыжка
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

        coyoteTimeCounter = 0f;
    }

    private void ContinueJump()
    {
        if (jumpTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Продолжаем прыжок
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

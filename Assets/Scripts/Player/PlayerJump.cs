using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // Публичные параметры прыжка
    public float jumpForce = 10f; // Сила прыжка
    public Transform groundCheck; // Точка проверки земли
    public LayerMask groundLayer; // Слой земли

    // Внутренние переменные
    private Rigidbody2D rb; // Физика персонажа
    private bool isGrounded; // Проверка контакта с землей
    public Animator animator; // Ссылка на аниматор

    // Настройки для кайот-тайм
    [Header("Настройки для кайот-тайм")]
    public float coyoteTime = 0.2f; // Время, когда можно прыгнуть после ухода с платформы
    private float coyoteTimeCounter; // Счетчик кайот-тайм

    // Настройки для двойных прыжков
    [Header("Настройки для количества прыжков")]
    public int maxAirJumps = 1; // Количество прыжков в воздухе
    private int remainingJumps; // Сколько прыжков осталось

    // Настройки для динамического прыжка
    [Header("Настройки для динамического прыжка")]
    public float jumpTimeMax = 0.3f; // Максимальное время удержания прыжка
    public float jumpCutMultiplier = 3f; // Множитель для прерывания прыжка
    private bool isJumping; // Флаг активного прыжка
    private float jumpTimeCounter; // Счетчик времени прыжка

    // Ссылка на систему голода
    private HungerSystem hungerSystem;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Инициализация физики
        hungerSystem = GetComponent<HungerSystem>(); // Инициализация системы голода
    }

    void Update()
    {
        // Проверка земли
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Обновляем параметры анимации
        animator.SetBool("Jumping", !isGrounded && rb.linearVelocity.y > 0);
        animator.SetBool("Falling", !isGrounded && rb.linearVelocity.y < 0);

        // Сбрасываем счетчики при касании земли
        if (isGrounded)
        {
            
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
                    hungerSystem.OnJump(); // Уменьшаем сытость при прыжке
                }
            }
        }

        // Продолжение прыжка
        if (Input.GetButton("Jump") && isJumping)
        {
            ContinueJump();
        }

        // Прерывание прыжка
        if (Input.GetButtonUp("Jump"))
        {
            EndJumpEarly();
        }
    }

    private void StartJump()
    {
        // Инициализируем прыжок
        isJumping = true;
        jumpTimeCounter = jumpTimeMax;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // Уменьшаем количество оставшихся прыжков, если находимся в воздухе
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

        // Обнуляем счетчик кайот-тайм
        coyoteTimeCounter = 0f;
    }

    private void ContinueJump()
    {
        // Продолжаем прыжок, пока есть запас времени
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
        // Преждевременно заканчиваем прыжок
        isJumping = false;
        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / jumpCutMultiplier);
        }
    }
}

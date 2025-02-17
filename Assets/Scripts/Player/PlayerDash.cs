using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Параметры рывка")]
    public float dashSpeed = 25f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool canDash = true;
    private bool isDashing;
    private float originalGravity;
    private HungerSystem hungerSystem;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Получаем компонент анимации
        originalGravity = rb.gravityScale;
        hungerSystem = GetComponent<HungerSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing && hungerSystem.CanDash())
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.gravityScale = 0;
        animator.SetBool("Dashing", true); // Включаем анимацию рывка

        hungerSystem.OnDash(); // Тратим сытость

        float direction = Input.GetAxisRaw("Horizontal");
        if (direction == 0) direction = transform.localScale.x;

        float elapsedTime = 0;
        while (elapsedTime < dashTime)
        {
            rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("Dashing", false); // Выключаем анимацию рывка

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void DisableDash()
    {
        canDash = false;
    }

    public void EnableDash()
    {
        canDash = true;
    }
}

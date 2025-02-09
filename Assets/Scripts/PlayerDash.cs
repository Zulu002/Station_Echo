using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    [Header("Параметры рывка")]
    public float dashSpeed = 25f;   // Скорость рывка
    public float dashTime = 0.2f;   // Время рывка
    public float dashCooldown = 0.5f; // Перезарядка

    private Rigidbody2D rb;
    private bool canDash = true;
    private bool isDashing;
    private float originalGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.gravityScale = 0;  // Отключаем гравитацию

        float direction = Input.GetAxisRaw("Horizontal");
        if (direction == 0) direction = transform.localScale.x; // Если стоим, берём направление персонажа

        float elapsedTime = 0;
        while (elapsedTime < dashTime)
        {
            rb.linearVelocity = new Vector2(direction * dashSpeed, 0); // Используем linearVelocity
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero; // Останавливаем движение после рывка
        rb.gravityScale = originalGravity; // Включаем гравитацию обратно

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

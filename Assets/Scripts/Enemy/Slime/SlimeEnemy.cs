using System.Collections;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public float speed = 2f; // Скорость движения
    public int maxHealth = 3; // Количество жизней
    public int damage = 3; // Урон игроку
    public float aggroRange = 5f; // Радиус агрессии

    public LayerMask playerLayer; // Слой игрока

    public AudioClip hitSound;
    public AudioClip deathSound;

    private int currentHealth;
    private Transform player;
    private bool isAggro = false;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth; // Устанавливаем полное здоровье
    }

    void Update()
    {
        if (isDead) return; // Если мертв, не делать ничего

        DetectPlayer();

        if (isAggro && player != null)
        {
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isMoving", false); // Переход в ожидание
        }
    }

    private void DetectPlayer()
    {
        Collider2D detected = Physics2D.OverlapCircle(transform.position, aggroRange, playerLayer);
        if (detected != null)
        {
            player = detected.transform;
            isAggro = true;
            animator.SetBool("isMoving", true); // Запускаем анимацию бега
        }
        else
        {
            isAggro = false;
        }
    }

    private void ChasePlayer()
    {
        // Вычисляем направление к игроку
        Vector2 direction = (player.position - transform.position).normalized;

        // Поворачиваем слайма в сторону игрока
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Поворот вправо
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Поворот влево
        }

        // Двигаемся к игроку
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return; // Если уже мертв, не реагировать

        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Если игрок падает сверху, слайм получает урон
                if (collision.contacts[0].normal.y < -0.5f)
                {
                    TakeDamage(1); // От удара сверху теряет 1 хп
                }
                else
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hit"); // Запускаем анимацию ранения

        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(RecoverFromHit());
        }
    }

    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Ждем завершения анимации "Hit"
        animator.ResetTrigger("Hit");
    }


    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        animator.SetTrigger("Death"); // Запускаем анимацию смерти
        rb.linearVelocity = Vector2.zero; // Полностью останавливаем
        rb.bodyType = RigidbodyType2D.Kinematic; // Отключаем физику
        GetComponent<Collider2D>().enabled = false; // Отключаем коллайдер

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); // Удаляем после анимации
    }

}

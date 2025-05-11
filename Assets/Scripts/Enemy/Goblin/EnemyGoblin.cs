using UnityEngine;
using System.Collections;

public class EnemyGoblin : MonoBehaviour
{

    public Transform pointA, pointB;
    public float speed = 2f;
    public float chaseSpeed = 3.5f;
    public float visionRange = 5f;
    public float attackRange = 1.5f;
    public int damage = 2;
    public int health = 3;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private Transform targetPoint;
    private GameObject player;
    private Animator animator;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isWaiting = false;
    private float lastAttackTime = 0f;

    void Start()
    {
        targetPoint = pointB;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead || isAttacking || isWaiting) return; // Если мертв, атакует или ждет — ничего не делаем

        DetectPlayer();

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);

        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.05f && !isWaiting)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(1f); // Гоблин стоит 1 секунду

        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        Flip();
        animator.SetBool("isWalking", true);
        isWaiting = false;
    }

    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);
        if (playerCollider != null)
        {
            player = playerCollider.gameObject;
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            StartCoroutine(AttackPlayer());
        }
        else if (distance >= attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }

        FlipTowards(player.transform.position);
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(0.6f); // Ждем завершения анимации атаки

        if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        yield return new WaitForSeconds(0.4f); // Добавляем задержку перед возвращением
        isAttacking = false;
        lastAttackTime = Time.time;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void FlipTowards(Vector3 target)
    {
        if ((target.x > transform.position.x && transform.localScale.x < 0) ||
            (target.x < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Проверка, что игрок прыгает сверху
            if (collision.transform.position.y > transform.position.y + 0.5f && collision.relativeVelocity.y > 0) // Игрок находится выше и двигается вниз
            {
                TakeDamage(health); // Убийство гоблина
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 5f); // Подбрасываем игрока
            }
            else
            {
                TakeDamage(1); // Урон от обычной атаки
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("Hit");

        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        animator.SetTrigger("Death");

        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject); // Удаляем объект гоблина
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using System.Collections;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    public float speed = 2f; 
    public int maxHealth = 3; 
    public int damage = 3; 
    public float aggroRange = 5f; 

    public LayerMask playerLayer; 

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
        currentHealth = maxHealth; 
    }

    void Update()
    {
        if (isDead) return; 

        DetectPlayer();

        if (isAggro && player != null)
        {
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isMoving", false); 
        }
    }

    private void DetectPlayer()
    {
        Collider2D detected = Physics2D.OverlapCircle(transform.position, aggroRange, playerLayer);
        if (detected != null)
        {
            player = detected.transform;
            isAggro = true;
            animator.SetBool("isMoving", true); 
        }
        else
        {
            isAggro = false;
        }
    }

    private void ChasePlayer()
    {
        
        Vector2 direction = (player.position - transform.position).normalized;

        
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); 
        }

        
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
        animator.SetTrigger("Hit"); 

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
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
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

        animator.SetTrigger("Death"); 
        rb.linearVelocity = Vector2.zero; 
        rb.bodyType = RigidbodyType2D.Kinematic; 
        GetComponent<Collider2D>().enabled = false; 

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); 
    }

}

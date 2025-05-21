using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxLives = 3;
    private int currentLives;

    public Image[] hearts;

    [Header("Звуки")]
    public AudioClip hitSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    void Start()
    {
        GameManager.Instance.player = this.gameObject;

        currentLives = maxLives;
        UpdateLivesUI();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.Play("Idle");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) TakeDamage(2);
        if (collision.CompareTag("DeathZone")) Die();
    }

    public void TakeDamage(int damage)
    {
        if (animator != null) animator.Play("Hit", -1, 0f);

        currentLives -= damage;
        UpdateLivesUI();

        if (hitSound != null && audioSource != null)
            audioSource.PlayOneShot(hitSound);

        if (currentLives <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Игрок погиб");

        if (animator != null) animator.SetTrigger("Death");

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;

        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI();
        }
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    public void RestoreAllLives()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }
}

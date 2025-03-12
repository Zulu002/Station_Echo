using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxLives = 3;
    private int currentLives;

    // ћассив изображений дл€ отображени€ жизней
    public Image[] hearts;

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) TakeDamage();
        if (collision.CompareTag("DeathZone")) Die();
    }

    private void TakeDamage()
    {
        currentLives--;
        UpdateLivesUI();
        Debug.Log($"Player hit! Lives remaining: {currentLives}");

        if (currentLives <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Player Died");

        if (animator != null) animator.SetTrigger("Death");

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
            Debug.Log($"Life added! Current lives: {currentLives}");
        }
    }

    private void UpdateLivesUI()
    {
        // ѕеребираем сердечки и включаем только те, которые соответствуют текущему количеству жизней
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    public void RestoreAllLives()
    {
        currentLives = maxLives; // ѕолное восстановление
        UpdateLivesUI();
        Debug.Log("¬се жизни восстановлены!");
    }

}

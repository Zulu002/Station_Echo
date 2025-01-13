using UnityEngine;
using UnityEngine.UI; // Для работы с UI
using UnityEngine.SceneManagement; // Подключаем SceneManager

public class PlayerHealth : MonoBehaviour
{
    public Animator animator; // Аниматор для анимации смерти
    public int maxLives = 3; // Максимальное количество жизней
    private int currentLives; // Текущее количество жизней

    public Text livesText; // UI элемент для отображения жизней

    void Start()
    {
        // Устанавливаем начальное количество жизней
        currentLives = maxLives;
        UpdateLivesUI(); // Обновляем UI
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, попал ли персонаж в объект с тегом "Hazard"
        if (collision.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(); // Уменьшаем количество жизней
        }
    }

    private void TakeDamage()
    {
        currentLives--; // Уменьшаем жизни на 1
        UpdateLivesUI(); // Обновляем UI
        Debug.Log($"Player hit! Lives remaining: {currentLives}");

        if (currentLives <= 0)
        {
            Die(); // Если жизни закончились, вызываем смерть
        }
        else
        {
            // Проигрываем анимацию "ранения" (опционально)
            if (animator != null)
            {
                // animator.SetTrigger("Hurt");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");

        // Проигрываем анимацию смерти
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Отключаем управление персонажем
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;

        // Перезапускаем уровень через 2 секунды
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddLife()
    {
        // Увеличиваем количество жизней, но не превышаем максимум
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI(); // Обновляем UI
            Debug.Log($"Life added! Current lives: {currentLives}");
        }
    }

    private void UpdateLivesUI()
    {
        // Обновляем текстовое поле
        if (livesText != null)
        {
            livesText.text = $"Lives: {currentLives}";
        }
    }
}

using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI
using UnityEngine.SceneManagement; // ���������� SceneManager

public class PlayerHealth : MonoBehaviour
{
    public Animator animator; // �������� ��� �������� ������
    public int maxLives = 3; // ������������ ���������� ������
    private int currentLives; // ������� ���������� ������

    public Text livesText; // UI ������� ��� ����������� ������

    void Start()
    {
        // ������������� ��������� ���������� ������
        currentLives = maxLives;
        UpdateLivesUI(); // ��������� UI
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Hazard"))
        {
            TakeDamage();
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Die(); 
        }
    }

    private void TakeDamage()
    {
        currentLives--; // ��������� ����� �� 1
        UpdateLivesUI(); // ��������� UI
        Debug.Log($"Player hit! Lives remaining: {currentLives}");

        if (currentLives <= 0)
        {
            Die(); // ���� ����� �����������, �������� ������
        }
        else
        {
            // ����������� �������� "�������" (�����������)
            if (animator != null)
            {
                // animator.SetTrigger("Hurt");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");

        // ����������� �������� ������
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // ��������� ���������� ����������
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerJump>().enabled = false;

        // ������������� ������� ����� 2 �������
        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        // ������������� ������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddLife()
    {
        // ����������� ���������� ������, �� �� ��������� ��������
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLivesUI(); // ��������� UI
            Debug.Log($"Life added! Current lives: {currentLives}");
        }
    }

    private void UpdateLivesUI()
    {
        // ��������� ��������� ����
        if (livesText != null)
        {
            livesText.text = $"Lives: {currentLives}";
        }
    }
}

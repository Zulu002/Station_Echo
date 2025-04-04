using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    public int maxLives = 3;
    private int currentLives;

    // ������ ����������� ��� ����������� ������
    public Image[] hearts;

    void Start()
    {
        GameManager.Instance.player = this.gameObject;
        transform.position = GameManager.Instance.lastCheckpointPosition;
        currentLives = maxLives;
        UpdateLivesUI();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.Play("Idle"); // ����� ����� ������������ � ����������� ��������
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard")) TakeDamage(1);
        if (collision.CompareTag("DeathZone")) Die();
    }

    public void TakeDamage(int damage)
    {
        if (animator != null)
        {
            animator.Play("Hit", -1, 0f);
        }

        currentLives -= damage; // �������� ��������� ���������� HP
        UpdateLivesUI();
        Debug.Log($"Player hit! Lives remaining: {currentLives}");

        if (currentLives <= 0)
        {
            Die();
        }
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
        // ���������� �������� � �������� ������ ��, ������� ������������� �������� ���������� ������
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentLives;
        }
    }

    public void RestoreAllLives()
    {
        currentLives = maxLives; // ������ ��������������
        UpdateLivesUI();
        Debug.Log("��� ����� �������������!");
    }

}

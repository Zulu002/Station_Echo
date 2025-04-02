using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public Transform player; // ������ �� ������
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���� ���� �����, ������������ NPC � ��� �������
        if (player != null)
        {
            FlipTowardsPlayer();
        }
    }

    private void FlipTowardsPlayer()
    {
        // ��������� ��������� ������ ������������ NPC
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // ������� ������
        }
        else
        {
            spriteRenderer.flipX = true; // ������� �����
        }
    }
}

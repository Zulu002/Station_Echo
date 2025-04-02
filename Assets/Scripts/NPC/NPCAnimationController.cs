using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Если есть игрок, поворачиваем NPC в его сторону
        if (player != null)
        {
            FlipTowardsPlayer();
        }
    }

    private void FlipTowardsPlayer()
    {
        // Проверяем положение игрока относительно NPC
        if (player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false; // Смотрим вправо
        }
        else
        {
            spriteRenderer.flipX = true; // Смотрим влево
        }
    }
}

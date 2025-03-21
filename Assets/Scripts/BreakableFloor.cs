using System.Collections;
using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    public float disappearTime = 2f; // Время до исчезновения
    public float respawnTime = 3f; // Время до появления

    private bool isBroken = false; // Флаг разрушения
    private Animator animator; // Ссылка на аниматор
    private Collider2D floorCollider; // Коллайдер платформы
    private SpriteRenderer spriteRenderer; // Спрайт платформы

    void Start()
    {
        animator = GetComponent<Animator>(); // Получаем компонент аниматора
        floorCollider = GetComponent<Collider2D>(); // Получаем коллайдер
        spriteRenderer = GetComponent<SpriteRenderer>(); // Получаем спрайт
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBroken && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BreakFloor());
        }
    }

    private IEnumerator BreakFloor()
    {
        isBroken = true; // Устанавливаем флаг разрушения
        animator.SetTrigger("Activate"); // Запускаем анимацию

        yield return new WaitForSeconds(disappearTime); // Ждем перед исчезновением

        // Отключаем коллайдер и делаем платформу невидимой
        floorCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime); // Ждем перед восстановлением

        // Включаем обратно
        floorCollider.enabled = true;
        spriteRenderer.enabled = true;
        animator.SetTrigger("Deactivate"); // Возвращаем анимацию в исходное состояние

        isBroken = false; // Сбрасываем флаг
    }
}

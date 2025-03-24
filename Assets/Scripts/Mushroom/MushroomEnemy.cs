using UnityEngine;
using System.Collections;

public class MushroomEnemy : MonoBehaviour
{
    public Transform pointA, pointB; // Точки движения
    public float speed = 2f; // Скорость передвижения
    public int damage = 3; // Урон игроку
    public float waitTime = 2f; // Время ожидания на точках

    private Transform targetPoint; // Куда движется гриб
    private Animator animator;
    private bool isWaiting = false; // Флаг, чтобы не двигаться во время ожидания

    void Start()
    {
        targetPoint = pointB; // Начинаем движение к точке B
        animator = GetComponent<Animator>(); // Получаем аниматор
        animator.SetBool("isMoving", true); // Запускаем анимацию ходьбы
    }

    void Update()
    {
        if (!isWaiting) Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Если дошли до точки — меняем направление и ждём
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        animator.SetBool("isMoving", false); // Переход в Idle
        yield return new WaitForSeconds(waitTime); // Ждём 2 секунды
        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        Flip();
        animator.SetBool("isMoving", true); // Включаем анимацию движения
        isWaiting = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Разворачиваем по оси X
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}

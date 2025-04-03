using UnityEngine;
using System.Collections;

public class MushroomEnemy : MonoBehaviour
{
    public Transform pointA, pointB; // ����� ��������
    public float speed = 2f; // �������� ������������
    public int damage = 3; // ���� ������
    public float waitTime = 2f; // ����� �������� �� ������

    private Transform targetPoint; // ���� �������� ����
    private Animator animator;
    private bool isWaiting = false; // ����, ����� �� ��������� �� ����� ��������

    void Start()
    {
        targetPoint = pointB; // �������� �������� � ����� B
        animator = GetComponent<Animator>(); // �������� ��������
        animator.SetBool("isMoving", true); // ��������� �������� ������
    }

    void Update()
    {
        if (!isWaiting) Move();
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // ���� ����� �� ����� � ������ ����������� � ���
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            StartCoroutine(WaitBeforeMoving());
        }
    }

    private IEnumerator WaitBeforeMoving()
    {
        isWaiting = true;
        animator.SetBool("isMoving", false); // ������� � Idle
        yield return new WaitForSeconds(waitTime); // ��� 2 �������
        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        Flip();
        animator.SetBool("isMoving", true); // �������� �������� ��������
        isWaiting = false;
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // ������������� �� ��� X
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

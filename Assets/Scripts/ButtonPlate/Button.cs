using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door; // ������ �� �����
    private Animator animator; // �������� ������
    private bool isActivated = false; // ����, ����� ��������� ������ ���� ���

    private void Start()
    {
        animator = GetComponent<Animator>(); // �������� �������� ������
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && (collision.CompareTag("Player") || collision.CompareTag("MovableObject")))
        {
            isActivated = true; // ����������, ��� ������ ��� ������
            animator.SetBool("isPressed", true); // ��������� ��������
            if (door != null)
                door.SetDoorState(true); // ��������� �����
        }
    }
}

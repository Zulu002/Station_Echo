using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 5f; // �������� ������� �� ��������
    private Rigidbody2D rb;
    private bool isClimbing = false; // ���� ��� �������� ���������� �� ��������
    private float originalGravity; // ��� ���������� ����������� ����������
    public Animator animator; // �������� ��� �������� �������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale; // ��������� �������� �������� ����������
    }

    void Update()
    {
        // ���� ����� ��������� �� ��������, ������������ ������
        if (isClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical"); // �������� ���� �� ���������

            // ���� ����� ��������� ����� ��� ����
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * climbSpeed);

            // ��������� ����������, ����� �� ������
            rb.gravityScale = 0;

            // ����������� �������� �������
            if (animator != null)
            {
                //animator.SetBool("Climbing", vertical != 0);
            }
        }
        else
        {
            // ��������������� ����������, ���� �� �� ��������
            rb.gravityScale = originalGravity;

            // ����������� �������� "�� �����������"
            if (animator != null)
            {
                //animator.SetBool("Climbing", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������, ����� �� ����� �� ������ � ����� "Ladder"
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ������� �� ��������� �������, ���� �������� ��������
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
}

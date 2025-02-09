using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 10f; // ���� ������
    public Transform groundCheck; // ����� �������� �����
    public LayerMask groundLayer; // ���� �����
    private Rigidbody2D rb;
    private bool isGrounded;
    public Animator animator;

    // ��������� ��� ���������
    public float coyoteTime = 0.2f; // �����, � ������� �������� ����� ������� ����� ������ �������� � ������
    private float coyoteTimeCounter; // ������ ���������

    // ��������� ��� ���������� �������
    public int maxAirJumps = 1; // ������������ ���������� ������� � �������
    private int remainingJumps; // ������� �������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���������, ��������� �� �������� �� �����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // ��������� �������� �������� Jumping � ���������
        animator.SetBool("Jumping", !isGrounded); // ������: ����� �� �� �����, �������� ��������

        // ������ ���������
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // ���� �������� �� �����, ��������� ������
            remainingJumps = maxAirJumps; // ���������� ���������� ������
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // ��������� ������, ���� �������� � �������
        }

        // ������
        if (Input.GetButtonDown("Jump"))
        {
            // ������ ��������, ����:
            // - ������� ����� �� ��������� (�.�. �������� ��� �� ����� �������)
            // - �������� ������ � �������
            if (coyoteTimeCounter > 0f || remainingJumps > 0)
            {
                // ��������� ������
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // ������ ������� � ������� � ������ ����������
                if (!isGrounded)
                {
                    if (coyoteTimeCounter > 0f)
                    {
                        // ���� �� ��������� � ����������, ���� ��� ���� �������������� ������
                        remainingJumps = maxAirJumps;
                    }
                    else if (remainingJumps > 0)
                    {
                        remainingJumps--; // ������� ������ � �������
                    }
                }

                // �������� ������ ���������, ����� �������� �������� ������ � �����
                coyoteTimeCounter = 0f;
            }
        }
    }
}

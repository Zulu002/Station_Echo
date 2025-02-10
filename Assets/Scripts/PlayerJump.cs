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
    [Header("��������� ��� �����-����")]
    public float coyoteTime = 0.2f; // �����, � ������� �������� ����� ������� ����� ������ �������� � ������
    private float coyoteTimeCounter; // ������ ���������

    
    [Header("��������� ��� ���������� �������")]
    public int maxAirJumps = 1; // ������������ ���������� ������� � �������
    private int remainingJumps; // ������� �������

    
    [Header("��������� ��� ������������� ������")]
    public float jumpTimeMax = 0.3f;       // ������������ ����� ��������� ������
    public float jumpCutMultiplier = 3f;   // �������� ���������� ��� ������ ����������
    private bool isJumping;
    private float jumpTimeCounter;

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

        // ������ ������
        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteTimeCounter > 0f || remainingJumps > 0)
            {
                StartJump();
            }
        }

        // ����������� ������ (������ ���������)
        if (Input.GetButton("Jump") && isJumping)
        {
            ContinueJump();
        }

        // ��������������� ��������� ������
        if (Input.GetButtonUp("Jump"))
        {
            EndJumpEarly();
        }
    }

    private void StartJump()
    {
        isJumping = true;
        jumpTimeCounter = jumpTimeMax;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (!isGrounded)
        {
            if (coyoteTimeCounter > 0f)
            {
                remainingJumps = maxAirJumps;
            }
            else if (remainingJumps > 0)
            {
                remainingJumps--;
            }
        }

        coyoteTimeCounter = 0f; // ����� ������� ���������
    }

    private void ContinueJump()
    {
        if (jumpTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
        }
    }

    private void EndJumpEarly()
    {
        isJumping = false;

        if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / jumpCutMultiplier);
        }
    }
}

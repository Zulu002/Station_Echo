using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    
    private Rigidbody2D rb; 
    private bool isGrounded; 
    public Animator animator; 

    [Header("Звук прыжка")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    
    [Header("Настройки для кайот-тайм")]
    public float coyoteTime = 0.2f; 
    private float coyoteTimeCounter; 

    
    [Header("Настройки для количества прыжков")]
    public int maxAirJumps = 1; 
    private int remainingJumps; 

    
    [Header("Настройки для динамического прыжка")]
    public float jumpTimeMax = 0.3f; 
    public float jumpCutMultiplier = 3f; 
    private bool isJumping; 
    private float jumpTimeCounter; 

    // Ссылка на систему голода
    private HungerSystem hungerSystem;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>(); 
        hungerSystem = GetComponent<HungerSystem>(); 
    }

    void Update()
    {
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        
        animator.SetBool("Jumping", !isGrounded && rb.linearVelocity.y > 0);
        animator.SetBool("Falling", !isGrounded && rb.linearVelocity.y < 0);

        
        if (isGrounded)
        {
            
            coyoteTimeCounter = coyoteTime;
            remainingJumps = maxAirJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }


        if (Input.GetButtonDown("Jump"))
        {
            
            if (isGrounded || coyoteTimeCounter > 0f)
            {
                StartJump();
            }
            
            else if (remainingJumps > 0 && hungerSystem != null && hungerSystem.CanJump())
            {
                StartJump();
                remainingJumps--;
                hungerSystem.OnJump();
            }
        }



        if (Input.GetButton("Jump") && isJumping)
        {
            ContinueJump();
        }

        
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

        
        coyoteTimeCounter = 0f;

        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
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

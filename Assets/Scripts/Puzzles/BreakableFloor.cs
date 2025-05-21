using System.Collections;
using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    public float disappearTime = 2f; 
    public float respawnTime = 3f; 
    public AudioClip breakSound; 

    private bool isBroken = false;
    private Animator animator;
    private Collider2D floorCollider;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        floorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
        isBroken = true;
        animator.SetTrigger("Activate");

        
        if (audioSource != null && breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }

        yield return new WaitForSeconds(disappearTime);

        floorCollider.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        floorCollider.enabled = true;
        spriteRenderer.enabled = true;
        animator.SetTrigger("Deactivate");

        isBroken = false;
    }
}

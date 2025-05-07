using UnityEngine;

public class HungerRestoration : MonoBehaviour
{
    public int amount_of_satiety = 20;
    public SpriteRenderer interactionHint; // —сылка на "E"
    public HungerSystem playerHunger;

    public AudioClip eatSound; //  лип звука еды
    private AudioSource audioSource;

    private bool isPlayerNearby = false;
    private bool isPlayingSound = false;

    private void Start()
    {
        interactionHint.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource не найден на объекте HungerRestoration.");
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKey(KeyCode.E))
        {
            playerHunger.IncreaseHunger(amount_of_satiety * Time.deltaTime);

            if (!isPlayingSound && eatSound != null && audioSource != null)
            {
                audioSource.clip = eatSound;
                audioSource.loop = true;
                audioSource.Play();
                isPlayingSound = true;
            }
        }
        else if (isPlayingSound && audioSource != null)
        {
            audioSource.Stop();
            isPlayingSound = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionHint.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionHint.enabled = false;

            if (isPlayingSound && audioSource != null)
            {
                audioSource.Stop();
                isPlayingSound = false;
            }
        }
    }
}

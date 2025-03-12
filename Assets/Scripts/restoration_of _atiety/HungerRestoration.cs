using UnityEngine;

public class HungerRestoration : MonoBehaviour
{
    public int amount_of_satiety = 20;
    public SpriteRenderer interactionHint; // ������ �� "E"
    public HungerSystem playerHunger;
    private bool isPlayerNearby = false;

    private void Start()
    {
        interactionHint.enabled = false; // �������� "E" ��� ������
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKey(KeyCode.E))
        {
            playerHunger.IncreaseHunger(amount_of_satiety * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionHint.enabled = true; // ���������� "E"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionHint.enabled = false; // �������� "E"
        }
    }
}

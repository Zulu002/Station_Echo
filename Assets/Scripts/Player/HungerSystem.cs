using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    public float maxHunger = 100f; 
    private float currentHunger; 

    public float hungerDecreaseRate = 0.1f; 
    public float jumpCost = 5f; 
    public float dashCost = 10f; 

    private bool isStarving = false; 

    private PlayerDash playerDash;
    private PlayerJump playerJump;

    public Image hungerImage; 

    private void Start()
    {
        currentHunger = maxHunger;
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();

        UpdateHungerUI(); // Инициализируем изображение
    }

    private void Update()
    {
        // Расход сытости при движении
        if (Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f)
        {
            ReduceHunger(hungerDecreaseRate * Time.deltaTime);
        }

        UpdateHungerUI(); // Обновляем UI каждый кадр
    }

    public void ReduceHunger(float amount)
    {
        currentHunger -= amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (currentHunger <= 0 && !isStarving)
        {
            isStarving = true;
            ApplyStarvationEffects();
        }
    }

    public void IncreaseHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (isStarving && currentHunger > 10)
        {
            isStarving = false;
            ResetStats();
        }
    }

    private void ApplyStarvationEffects()
    {
        playerDash.DisableDash();
    }

    private void ResetStats()
    {
        playerDash.EnableDash();
    }

    public bool CanJump()
    {
        return currentHunger > 0;
    }

    public bool CanDash()
    {
        return currentHunger > 0;
    }

    public void OnJump()
    {
        if (CanJump())
        {
            ReduceHunger(jumpCost);
        }
    }

    public void OnDash()
    {
        if (CanDash())
        {
            ReduceHunger(dashCost);
        }
    }

    private void UpdateHungerUI()
    {
        if (hungerImage != null)
        {
            float alpha = currentHunger / maxHunger; // Вычисляем уровень прозрачности
            hungerImage.color = new Color(hungerImage.color.r, hungerImage.color.g, hungerImage.color.b, alpha);
        }
    }
}

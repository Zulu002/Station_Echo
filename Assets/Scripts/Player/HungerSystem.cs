using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    public float maxHunger = 100f; // Максимальная сытость
    private float currentHunger; // Текущая сытость

    public float hungerDecreaseRate = 0.1f; // Расход при ходьбе
    public float jumpCost = 5f; // Расход за прыжок
    public float dashCost = 10f; // Расход за рывок

    private bool isStarving = false; // Флаг голодания

    private PlayerDash playerDash;
    private PlayerJump playerJump;

    public Slider hungerSlider; // UI-слайдер для шкалы сытости

    private void Start()
    {
        currentHunger = maxHunger;
        playerDash = GetComponent<PlayerDash>();
        playerJump = GetComponent<PlayerJump>();

        // Инициализируем слайдер
        if (hungerSlider != null)
        {
            hungerSlider.maxValue = maxHunger;
            hungerSlider.value = currentHunger;
            hungerSlider.direction = Slider.Direction.BottomToTop; // Вертикальное заполнение
        }
    }

    private void Update()
    {
        // Расход сытости при движении
        if (Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f)
        {
            ReduceHunger(hungerDecreaseRate * Time.deltaTime);
        }

        // Обновляем UI-слайдер
        if (hungerSlider != null)
        {
            hungerSlider.value = currentHunger;
        }
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
        // Отключаем рывок при голоде
        playerDash.DisableDash();
    }

    private void ResetStats()
    {
        // Включаем рывок обратно
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
}

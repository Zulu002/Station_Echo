using UnityEngine;
using UnityEngine.UI;  // Для работы с UI Text

public class LevelTrigger : MonoBehaviour
{
    public Timer timer;  // Ссылка на скрипт секундомера
    public Text timeDisplay;  // Ссылка на UI Text для отображения времени

    // Проверка на вход в триггер
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверка, что в триггер зашел объект с тегом "Player"
        if (other.CompareTag("Player"))
        {
            // Записываем время, которое прошло до входа в триггер
            timer.recordedTime = timer.GetTimeElapsed();
            Debug.Log("Время круга: " + timer.recordedTime + " сек");

            // Обновляем текстовое поле с сохраненным временем
            timeDisplay.text = "Время круга: " + timer.recordedTime.ToString("F2") + " сек";

            timer.ResetTimer();  // Сбрасываем таймер
        }
    }
}

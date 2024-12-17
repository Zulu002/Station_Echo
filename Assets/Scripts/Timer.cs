using UnityEngine;
using UnityEngine.UI;  // Если хотите отображать время на UI

public class Timer : MonoBehaviour
{
    public Text timerText;  // Ссылка на UI элемент для отображения времени
    private float timeElapsed = 0f;
    private bool isTiming = false;

    public float recordedTime = 0f;  // Переменная для сохранения времени, прошедшего до входа в триггер

    // Метод для начала отсчета времени
    public void StartTimer()
    {
        isTiming = true;
        timeElapsed = 0f;
    }

    // Метод для остановки секундомера
    public void StopTimer()
    {
        isTiming = false;
    }

    // Метод для сброса времени
    public void ResetTimer()
    {
        timeElapsed = 0f;
    }

    // Публичный метод для получения времени
    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    // Метод для обновления времени каждый кадр
    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;  // Увеличиваем время на время, прошедшее с последнего кадра
            timerText.text = "Time: " + timeElapsed.ToString("F2");  // Обновляем текст UI
        }
    }

    // Начать отсчет времени при старте игры
    void Start()
    {
        StartTimer();  // Начинаем отсчет времени сразу при старте игры
    }
}

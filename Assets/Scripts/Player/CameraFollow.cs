using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 3f, -10f); // Смещение камеры
    public float smoothTime = 0.3f; // Время сглаживания (меньше значения — быстрее реагирует)
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target; // Ссылка на игрока

    private void FixedUpdate() // Используем FixedUpdate вместо LateUpdate
    {
        if (target == null) return; // Защита от ошибок, если цель не задана

        // Целевая позиция камеры
        Vector3 targetPosition = target.position + offset;

        // Плавное движение камеры
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

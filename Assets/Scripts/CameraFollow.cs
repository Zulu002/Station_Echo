using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Ссылка на игрока
    public float smoothSpeed = 0.125f; // Скорость плавного следования
    public Vector3 offset; // Смещение камеры относительно игрока

    void LateUpdate()
    {
        // Вычисляем целевую позицию камеры
        Vector3 desiredPosition = player.position + offset;

        // Плавно изменяем позицию камеры
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Обновляем позицию камеры
        transform.position = smoothedPosition;
    }
}

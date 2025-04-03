using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public Vector3 moveOffset; // Насколько сдвигать дверь (локальное смещение)
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;

    public float openSpeed = 2f; // Скорость открытия
    public float closeSpeed = 2f; // Скорость закрытия

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + moveOffset; // Вычисляем конечную позицию двери
    }

    public void SetDoorState(bool open)
    {
        isOpen = open;
        StopAllCoroutines();
        StartCoroutine(MoveDoor(isOpen ? openPosition : closedPosition, isOpen ? openSpeed : closeSpeed));
    }

    private IEnumerator MoveDoor(Vector3 targetPos, float speed)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos; // Фиксируем позицию в конце
    }
}

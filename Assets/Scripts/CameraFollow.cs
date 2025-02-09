using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 3f, -10f); // �������� ������
    public float smoothTime = 0.3f; // ����� ����������� (������ �������� � ������� ���������)
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target; // ������ �� ������

    private void FixedUpdate() // ���������� FixedUpdate ������ LateUpdate
    {
        if (target == null) return; // ������ �� ������, ���� ���� �� ������

        // ������� ������� ������
        Vector3 targetPosition = target.position + offset;

        // ������� �������� ������
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

using UnityEngine;

public class StartupResolution : MonoBehaviour
{
    void Awake()
    {
        // ��������� 1920x1080 � ������������� ������ ��� ������ ����
        Screen.SetResolution(1920, 1080, true);
    }
}
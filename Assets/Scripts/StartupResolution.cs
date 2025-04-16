using UnityEngine;

public class StartupResolution : MonoBehaviour
{
    void Awake()
    {
        // Применить 1920x1080 в полноэкранном режиме при старте игры
        Screen.SetResolution(1920, 1080, true);
    }
}
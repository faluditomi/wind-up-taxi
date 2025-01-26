using UnityEngine;

public class FPSCap : MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 144;
    }
}

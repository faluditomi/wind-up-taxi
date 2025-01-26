using UnityEngine;

public class ChangeDeathScene : MonoBehaviour
{
    [SerializeField] Transform outOfTimeTransform;
    private Transform cameraTransform;

    [SerializeField] DeathMenu deathMenuScript;
    [SerializeField] PauseMenu pauseMenuScript;

    [SerializeField] GameObject deathCanvas;
    [SerializeField] GameObject defaultCanvas;
    private GameObject cinemachineChamera;

    public enum Reason
    {
        OutOfTime,
        CrashIntoCar,
        CrashIntoRobot,
        CrashIntoBuilding,
        Overcharged,
        Kidnapping
    }

    private void Awake()
    {
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();

        cinemachineChamera = GameObject.Find("CinemachineCamera");
    }

    public void ChangeCamera(Reason reason)
    {
        cinemachineChamera.SetActive(false);

        defaultCanvas.SetActive(false);

        deathCanvas.SetActive(true);

        switch(reason)
        {
            case Reason.OutOfTime:
                deathMenuScript.SetReason("You ran out of time");
                transform.SetParent(outOfTimeTransform);
                transform.localPosition = Vector3.zero;
            break;
            
            case Reason.CrashIntoBuilding:

            break;

            case Reason.CrashIntoRobot:

            break;

            case Reason.CrashIntoCar:

            break;

            case Reason.Overcharged:

            break;

            case Reason.Kidnapping:

            break;
        }

        cameraTransform.SetParent(transform);

        cameraTransform.localPosition = Vector3.zero;

        cameraTransform.localRotation = Quaternion.Euler(13.5f, 0, 0);
    }
}

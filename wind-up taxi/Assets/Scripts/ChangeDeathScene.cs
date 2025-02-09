using UnityEngine;
using FMODUnity;

public class ChangeDeathScene : MonoBehaviour
{
    [SerializeField] Transform outOfTimeTransform;
    [SerializeField] Transform crashIntoBuildingTransform;
    [SerializeField] Transform crashIntoCarTransform;
    [SerializeField] Transform overchargedTransform;
    [SerializeField] Transform kidnappingTransform;
    private Transform cameraTransform;

    private StudioEventEmitter soundEmitter;

    [SerializeField] DeathMenu deathMenuScript;
    [SerializeField] PauseMenu pauseMenuScript;
    private CarStateController carStateController;
    private Arrow arrowScript;

    [SerializeField] GameObject deathCanvas;
    [SerializeField] GameObject defaultCanvas;
    private GameObject cinemachineChamera;

    public enum Reason
    {
        OutOfTime,
        CrashIntoCar,
        CrashIntoBuilding,
        Overcharged,
        Kidnapping
    }

    private void Awake()
    {
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();
        arrowScript = FindFirstObjectByType<Arrow>();
        cinemachineChamera = GameObject.Find("CinemachineCamera");
        carStateController = FindAnyObjectByType<CarStateController>();
        soundEmitter = GameObject.Find("Music").GetComponent<StudioEventEmitter>();
    }

    public void ChangeCamera(Reason reason)
    {
        if(Leaderboard.Instance != null)
        {
            Leaderboard.Instance.AddScore(arrowScript.GetFinalScore());
        }

        cinemachineChamera.SetActive(false);

        defaultCanvas.SetActive(false);

        deathCanvas.SetActive(true);

        switch(reason)
        {
            case Reason.OutOfTime:
                deathMenuScript.SetReason("- Obstruction of Traffic\r\n- Failure to Act\r\n- Fraud");

                transform.SetParent(outOfTimeTransform);
            break;
            
            case Reason.CrashIntoBuilding:
                deathMenuScript.SetReason("- Reckless Driving\r\n- Criminal Mischief\r\n- Endangering Others\r\n- Driving Under the Influence\r\n- Insurance Fraud");

                transform.SetParent(crashIntoBuildingTransform);
            break;

            case Reason.CrashIntoCar:
                deathMenuScript.SetReason("- Reckless Driving\r\n- Vehicular Assault\r\n- Driving Under the Influence\r\n- Insurance Fraud");

                transform.SetParent(crashIntoCarTransform);
            break;

            case Reason.Overcharged:
                deathMenuScript.SetReason("- Obstruction of Traffic\r\n- Vehicular Negligence");

                transform.SetParent(overchargedTransform);
            break;

            case Reason.Kidnapping:
                deathMenuScript.SetReason("- Kidnapping\r\n- Robot Trafficking\r\n- Conspiracy");

                transform.SetParent(kidnappingTransform);
            break;
        }

        soundEmitter.SetParameter("Speed", -1);

        carStateController.SetState(CarStateController.CarState.Busted);

        transform.localPosition = Vector3.zero;

        cameraTransform.SetParent(transform);

        cameraTransform.localPosition = Vector3.zero;

        cameraTransform.localRotation = Quaternion.Euler(13.5f, 0, 0);
    }
}

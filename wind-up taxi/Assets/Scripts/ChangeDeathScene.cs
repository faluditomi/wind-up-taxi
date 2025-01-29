using UnityEngine;

public class ChangeDeathScene : MonoBehaviour
{
    private Arrow arrow;

    [SerializeField] Transform outOfTimeTransform;
    [SerializeField] Transform crashIntoBuildingTransform;
    [SerializeField] Transform crashIntoRobotTransform;
    [SerializeField] Transform crashIntoCarTransform;
    [SerializeField] Transform overchargedTransform;
    [SerializeField] Transform kidnappingTransform;
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
        arrow = FindFirstObjectByType<Arrow>();
        cinemachineChamera = GameObject.Find("CinemachineCamera");
    }

    public void ChangeCamera(Reason reason)
    {
        if(Leaderboard.Instance != null)
        {
            Leaderboard.Instance.AddScore(arrow.GetFinalScore());
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

            case Reason.CrashIntoRobot:
                deathMenuScript.SetReason("- Vehicular Botslaughter\r\n- Assault with a Deadly Weapon\r\n- Reckless Endangerment\r\n- Driving Under the Influence");

                transform.SetParent(crashIntoRobotTransform);
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

        transform.localPosition = Vector3.zero;

        cameraTransform.SetParent(transform);

        cameraTransform.localPosition = Vector3.zero;

        cameraTransform.localRotation = Quaternion.Euler(13.5f, 0, 0);
    }
}

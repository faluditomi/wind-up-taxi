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

    [SerializeField] float rotationSpeed;

    private enum Reason
    {
        OutOfTime,
        Crash
    }

    private void Awake()
    {
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();

        cinemachineChamera = GameObject.Find("CinemachineCamera");
    }

    public void ChangeCamera(string reason)
    {
        cinemachineChamera.SetActive(false);

        defaultCanvas.SetActive(false);

        deathCanvas.SetActive(true);

        if(reason == Reason.OutOfTime.ToString())
        {
            deathMenuScript.SetReason("You ran out of time");

            transform.SetParent(outOfTimeTransform);

            transform.localPosition = Vector3.zero;
        }

        cameraTransform.SetParent(transform);

        cameraTransform.localPosition = Vector3.zero;
    }
}

using UnityEngine;

public class ChangeDeathScene : MonoBehaviour
{
    [SerializeField] Transform outOfTimeTransform;
    private Transform cameraTransform;

    [SerializeField] DeathMenu deathMenuScript;
    [SerializeField] PauseMenu pauseMenuScript;

    [SerializeField] GameObject deathCanvas;
    [SerializeField] GameObject defaultCanvas;

    [SerializeField] float rotationSpeed;

    private bool isDead;

    private enum Reason
    {
        OutOfTime,
        Crash
    }

    private void Awake()
    {
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();
    }

    private void Update()
    {
        if(isDead)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    public void ChangeCamera(string reason)
    {
        defaultCanvas.SetActive(false);

        deathCanvas.SetActive(true);

        isDead = true;

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

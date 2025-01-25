using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform carTransform;
    private Transform currentDestination;

    private PauseMenu pauseMenuScript;

    private GameObject[] destinationObjects;

    [SerializeField] Material arrowMaterial;

    [SerializeField] float minDistance;
    [SerializeField] float baseBonusTime;
    [SerializeField] float timerModifier;

    private int scoreCounter = 1;
    private int passengersDelivered = 0;

    private bool isPassenger;

    public Vector3 arrowOffset = new Vector3(0, 1, 0);

    private void Awake()
    {
        carTransform = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();

        destinationObjects = GameObject.FindGameObjectsWithTag("Destination");

        pauseMenuScript = FindAnyObjectByType<PauseMenu>();
    }

    private void Start()
    {
        currentDestination = destinationObjects[Random.Range(0, destinationObjects.Length)].transform;

        arrowMaterial.color = Color.red;
    }

    private void Update()
    {
        transform.position = carTransform.position + arrowOffset;

        Vector3 direction = currentDestination.position - transform.position;

        direction.y = 0;

        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void SelectRandomDestination()
    {
        Transform newDestination;

        if(destinationObjects.Length > 0)
        {
            newDestination = destinationObjects[Random.Range(0, destinationObjects.Length)].transform;

            if(newDestination == currentDestination || Vector3.Distance(newDestination.position, currentDestination.position) < minDistance)
            {
                Debug.Log("Retry");

                SelectRandomDestination();
            }
            else
            {
                currentDestination = newDestination;

                Debug.Log("Next destination: " + currentDestination);
            }
        }
    }

    //temp: This should be in the car script. Remove Collider and Rigidbody from arrow prefab after.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination") && other.transform == GetCurrentDestination())
        {
            if(!GetIsPassenger())
            {
                PickupPassenger();
            }
            else
            {
                DeliverPassenger();
            }
        }
    }

    public void PickupPassenger()
    {
        Debug.Log("Choosing passenger.");

        isPassenger = true;

        arrowMaterial.color = Color.green;

        SelectRandomDestination();
    }

    public void DeliverPassenger()
    {
        Debug.Log("You delivered the passenger successfully.");

        isPassenger = false;

        passengersDelivered++;

        arrowMaterial.color = Color.red;

        float distance = Vector3.Distance(currentDestination.position, carTransform.position);

        float bonusTime = Mathf.Max(baseBonusTime + (distance * 0.5f * (1 - passengersDelivered * timerModifier)), 0);

        Debug.Log("Bonus time: " + bonusTime);

        pauseMenuScript.AddScore(scoreCounter);

        pauseMenuScript.AddTime(bonusTime);

        SelectRandomDestination();
    }

    public Transform GetCurrentDestination()
    {
        return currentDestination;
    }

    public bool GetIsPassenger()
    {
        return isPassenger;
    }
}

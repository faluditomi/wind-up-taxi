using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform carTransform;
    public Transform currentDestination;

    private PauseMenu pauseMenuScript;

    private GameObject[] destinationObjects;

    [SerializeField] float minDistance;
    [SerializeField] float bonusTime;

    private int scoreCounter = 1;

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination") && other.transform == currentDestination)
        {
            if(!isPassenger)
            {
                //Pickup passenger
                Debug.Log("Choosing passenger.");

                isPassenger = true;
            }
            else
            {
                //Deliver passenger
                pauseMenuScript.AddScore(scoreCounter);

                pauseMenuScript.AddTime(bonusTime);

                Debug.Log("You delivered the passenger successfully.");

                isPassenger = false;
            }

            SelectRandomDestination();
        }
    }
}

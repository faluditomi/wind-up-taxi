using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform carTransform;
    public Transform currentDestination;

    private GameObject[] destinationObjects;

    [SerializeField] float minDistance;

    private bool isPassenger;

    public Vector3 arrowOffset = new Vector3(0, 1, 0);

    private void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();

        destinationObjects = GameObject.FindGameObjectsWithTag("Destination");

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
        if(other.CompareTag("Destination"))
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

                //give point
                //recalculate timer

                Debug.Log("You delivered the passenger successfully.");

                isPassenger = false;
            }

            SelectRandomDestination();
        }
    }
}

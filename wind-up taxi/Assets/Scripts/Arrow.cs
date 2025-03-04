using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] Transform startingDestination;
    private Transform carTransform;
    private Transform currentDestination;
    private Transform currentPassenger;

    [SerializeField] DeathMenu deathMenuScript;
    private PauseMenu pauseMenuScript;

    private GameObject[] destinationObjects;
    
    private GameObject dropOffArrow;

    [SerializeField] Material arrowMaterial;

    [SerializeField] float minDistance;
    [SerializeField] float baseBonusTime;
    [SerializeField] float timerModifier;

    private int passengersDelivered = 0;
    private int finalScore = 0;

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
        SelectRandomDestination();

        arrowMaterial.color = Color.red;

        deathMenuScript.SetFinalScore(finalScore.ToString());

        currentDestination.Find("Passenger").gameObject.SetActive(true);
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

        if(dropOffArrow)
        {
            dropOffArrow.transform.Rotate(0, 0, 50 * Time.deltaTime);
        }
    }

    public int GetFinalScore()
    {
        return finalScore;
    }

    public bool HasPassanger()
    {
        return isPassenger;
    }

    private void SelectRandomDestination()
    {
        Transform newDestination;

        if(destinationObjects.Length > 0)
        {
            if(currentDestination == null)
            {
                currentDestination = startingDestination;

                Debug.Log("Next destination: " + currentDestination);
            }
            else
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
    }

    public void PickupPassenger()
    {
        Debug.Log("Choosing passenger.");

        isPassenger = true;

        arrowMaterial.color = Color.green;

        currentDestination.Find("Passenger").gameObject.SetActive(false);

        currentPassenger = currentDestination.GetComponentInChildren<Transform>();

        currentPassenger.gameObject.SetActive(false);

        SelectRandomDestination();

        CalculateTime();

        dropOffArrow = currentDestination.Find("Arrow").gameObject;
        
        dropOffArrow.gameObject.SetActive(true);
    }

    public void DeliverPassenger()
    {
        Debug.Log("You delivered the passenger successfully.");

        isPassenger = false;

        passengersDelivered++;

        arrowMaterial.color = Color.red;

        pauseMenuScript.AddScore(1);

        finalScore++;

        deathMenuScript.SetFinalScore(finalScore.ToString());

        currentPassenger.gameObject.SetActive(true);

        dropOffArrow.gameObject.SetActive(false);

        SelectRandomDestination();

        CalculateTime();

        currentDestination.Find("Passenger").gameObject.SetActive(true);
    }

    private void CalculateTime()
    {
        float distance = Vector3.Distance(currentDestination.position, carTransform.position);

        float bonusTime = Mathf.Max(baseBonusTime + (distance * 0.5f * (1 - passengersDelivered * timerModifier)), 0);

        Debug.Log("Bonus time: " + bonusTime);

        pauseMenuScript.AddTime(bonusTime);
    }

    public Transform GetCurrentDestination()
    {
        return currentDestination;
    }

    public bool GetIsPassenger()
    {
        return isPassenger;
    }

    public void ResetMap()
    {
        if(currentPassenger != null)
        {
            currentPassenger.gameObject.SetActive(true);

            currentDestination.Find("Passenger").gameObject.SetActive(false);
        }

        if(dropOffArrow != null)
        {
            dropOffArrow.gameObject.SetActive(false);
        }

        currentDestination = null;

        currentPassenger = null;

        passengersDelivered = 0;

        finalScore = 0;

        deathMenuScript.SetFinalScore(finalScore.ToString());

        isPassenger = false;

        SelectRandomDestination();

        arrowMaterial.color = Color.red;

        currentDestination.Find("Passenger").gameObject.SetActive(true);
    }
}

using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform carTransform;
    private Transform currentDestination;
    private Transform currentPassenger;

    [SerializeField] DeathMenu deathMenuScript;
    private PauseMenu pauseMenuScript;

    private GameObject[] destinationObjects;

    private GameObject dropOffArrow;

    [SerializeField] Material arrowMaterial;
    [SerializeField] Material outlineMaterial;
    private Material[] originalMaterials;

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

        originalMaterials = currentDestination.GetComponentInChildren<Renderer>().materials;

        AddMaterial();
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

    private void SelectRandomDestination()
    {
        Transform newDestination;

        if(destinationObjects.Length > 0)
        {
            if(currentDestination == null)
            {
                float closestDistance = float.MaxValue;

                Transform closestDestination = null;

                foreach(GameObject destinationObject in destinationObjects)
                {
                    float distance = Vector3.Distance(transform.position, destinationObject.transform.position);

                    if(distance < closestDistance)
                    {
                        closestDistance = distance;

                        closestDestination = destinationObject.transform;
                    }
                }

                currentDestination = closestDestination;

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

    private void AddMaterial()
    {
        if(!isPassenger)
        {
            Material[] currentMaterials = currentDestination.GetComponentInChildren<Renderer>().materials;
            Material[] updatedMaterials = new Material[currentMaterials.Length + 1];

            for(int i = 0; i < currentMaterials.Length; i++)
            {
                updatedMaterials[i] = currentMaterials[i];
            }

            updatedMaterials[updatedMaterials.Length - 1] = outlineMaterial;

            currentDestination.GetComponentInChildren<Renderer>().materials = updatedMaterials;
        }
    }
    

    public void PickupPassenger()
    {
        Debug.Log("Choosing passenger.");

        isPassenger = true;

        arrowMaterial.color = Color.green;

        currentDestination.GetComponentInChildren<Renderer>().materials = originalMaterials;

        currentPassenger = currentDestination.GetComponentInChildren<Transform>();

        currentPassenger.gameObject.SetActive(false);

        SelectRandomDestination();

        dropOffArrow = currentDestination.Find("Arrow").gameObject;
        
        dropOffArrow.gameObject.SetActive(true);

        currentDestination.Find("Passenger").gameObject.SetActive(false);
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

        pauseMenuScript.AddScore(1);

        finalScore++;

        deathMenuScript.SetFinalScore(finalScore.ToString());

        pauseMenuScript.AddTime(bonusTime);

        currentPassenger.gameObject.SetActive(true);

        dropOffArrow.gameObject.SetActive(false);

        currentDestination.Find("Passenger").gameObject.SetActive(true);

        SelectRandomDestination();

        AddMaterial();
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

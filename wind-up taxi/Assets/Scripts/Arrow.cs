using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Transform destinationTransform;
    private Transform carTransform;

    public Vector3 arrowOffset = new Vector3(0, 1, 0);

    private void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();
    }

    private void Update()
    {
        //Position the arrow above the car.
        transform.position = carTransform.position + arrowOffset;

        //Make the arrow point towards the destination.
        Vector3 direction = destinationTransform.position - transform.position;
        direction.y = 0;
        if(direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

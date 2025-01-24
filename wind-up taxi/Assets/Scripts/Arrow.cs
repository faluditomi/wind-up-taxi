using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
    private Transform carTransform;
    public Transform currentDestination;

    private GameObject[] destinationObjects;

    public Vector3 arrowOffset = new Vector3(0, 1, 0);

    private void Start()
    {
        carTransform = GameObject.FindGameObjectWithTag("Car").GetComponent<Transform>();

        destinationObjects = GameObject.FindGameObjectsWithTag("Destination");

        //temp
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

        //temp
        if(Input.GetKeyDown(KeyCode.P))
        {
            SelectRandomDestination();
        }
    }

    private void SelectRandomDestination()
    {
        Transform newDestination;

        if(destinationObjects.Length > 0)
        {
            newDestination = destinationObjects[Random.Range(0, destinationObjects.Length)].transform;

            currentDestination = newDestination;
        }
    }
}

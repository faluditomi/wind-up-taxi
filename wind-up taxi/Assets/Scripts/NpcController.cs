using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    private int currentWaypointIndex = 0;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    public float speed;

    private void Awake()
    {
        //transform.position = waypoints[0].position;
    }

    private void FixedUpdate()
    {
        if(waypoints.Count == 0)
        {
            return;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.fixedDeltaTime;
        transform.forward = direction;

        if(Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if(currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}

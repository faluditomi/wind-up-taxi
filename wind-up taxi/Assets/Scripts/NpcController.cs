using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NpcController : MonoBehaviour
{
    private int currentWaypointIndex = 0;

    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [SerializeField] float speed;
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    private float nextTime;

    private StudioEventEmitter honkEmitter;

    private void Awake()
    {
        honkEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if(this.gameObject.tag == "Honk")
        {
            //ScheduleNextHonk();
            nextTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    private void Update()
    {
        if(this.gameObject.tag == "Honk")
        {
            if(Time.time >= nextTime)
            {
                Debug.Log(this.gameObject + " honked!");

                honkEmitter.Play();

                //ScheduleNextHonk();
                nextTime = Time.time + Random.Range(minInterval, maxInterval);
            }
        }
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

    private void ScheduleNextHonk()
    {
        nextTime = Time.time + Random.Range(minInterval, maxInterval);
    }
}

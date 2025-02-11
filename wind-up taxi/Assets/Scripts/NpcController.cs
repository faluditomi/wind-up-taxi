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
    private float startingSpeed;

    private StudioEventEmitter honkEmitter;

    private void Awake()
    {
        honkEmitter = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        startingSpeed = speed;

        if(this.gameObject.tag == "Honk")
        {
            nextTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    private void Update()
    {
        if(this.gameObject.tag == "Honk")
        {
            if(Time.time >= nextTime)
            {
                honkEmitter.Play();

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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Car" || other.gameObject.tag == "Robot")
        {
            speed = 0f;

            if(!honkEmitter.IsPlaying())
            {
                honkEmitter.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Car" || other.gameObject.tag == "Robot")
        {
            speed = startingSpeed;
        }
    }
}

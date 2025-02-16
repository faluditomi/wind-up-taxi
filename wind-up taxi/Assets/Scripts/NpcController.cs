using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NpcController : MonoBehaviour
{
    private enum NpcType
    {
        Pedestrian,
        Car,
        FlyingThingy
    }

    [SerializeField] private NpcType npcType = NpcType.Pedestrian;

    private int currentWaypointIndex = 0;

    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [SerializeField] float speed;
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    private float nextTime;
    private float startingSpeed;

    private float flyingWobbleTime;
    private float flyingWobbleDuration;
    private float flyingWobbleDistance;

    private float pedestrianWobbleTime;
    private float pedestrianWobbleDuration;
    private float pedestrianWobbleDistance;

    private StudioEventEmitter honkEmitter;

    private void Awake()
    {
        honkEmitter = GetComponent<StudioEventEmitter>();

        pedestrianWobbleTime = Random.Range(-200, 200) * 0.01f;
        pedestrianWobbleDuration = Random.Range(50f, 250f) * 0.01f;
        pedestrianWobbleDistance = Random.Range(8, 16);

        flyingWobbleTime = Random.Range(-200, 200) * 0.01f;
        flyingWobbleDuration = Random.Range(50f, 250f) * 0.01f;
        flyingWobbleDistance = Random.Range(0, 41) * 0.01f;
    }

    private void Start()
    {
        startingSpeed = speed;
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

        if(npcType == NpcType.Pedestrian)
        {
            pedestrianWobbleTime += Time.deltaTime * pedestrianWobbleDuration;
            float zRotation = Mathf.Sin(pedestrianWobbleTime) * pedestrianWobbleDistance;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, zRotation);
        }
        else if(npcType == NpcType.FlyingThingy)
        {
            flyingWobbleTime += Time.deltaTime * flyingWobbleDuration;
            float yposition = Mathf.Sin(flyingWobbleTime) * flyingWobbleDistance;
            transform.position = new Vector3(transform.position.x, yposition, transform.position.z);
        }

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

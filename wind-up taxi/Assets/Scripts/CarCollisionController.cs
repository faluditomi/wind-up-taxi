using FMODUnity;
using UnityEngine;

public class CarCollisionController : MonoBehaviour
{
    private ChangeDeathScene deathScript;

    private void Awake()
    {
        deathScript = FindFirstObjectByType<ChangeDeathScene>();
    }

    private void OnCollisionEnter(Collision other)
    {
        switch(other.collider.tag)
        {
            case "Building":
                deathScript.ChangeCamera(ChangeDeathScene.Reason.CrashIntoBuilding);
                
            break;
            
            case "NpcCar":
                deathScript.ChangeCamera(ChangeDeathScene.Reason.CrashIntoCar);
            break;
            
            case "Robot":
                MeshCollider collider = other.gameObject.GetComponentInChildren<MeshCollider>();

                StudioEventEmitter emitter = other.gameObject.GetComponent<StudioEventEmitter>();
                emitter.Play();

                collider.isTrigger = true;

                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

                rb.useGravity = true;

                Destroy(other.gameObject, 10f);
            break;

            case "Destructable":
                BoxCollider boxCollider = other.gameObject.GetComponent<BoxCollider>();

                boxCollider.isTrigger = true;

                Destroy(other.gameObject, 10f);
                break;

            default:
            break;
        }
    }
}

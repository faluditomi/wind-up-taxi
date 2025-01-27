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
                MeshCollider collider = other.gameObject.GetComponent<MeshCollider>();
                
                collider.isTrigger = true;

                Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>();

                rb.useGravity = true;

                Destroy(other.transform.parent.gameObject, 10f);
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

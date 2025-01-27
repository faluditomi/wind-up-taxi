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
                deathScript.ChangeCamera(ChangeDeathScene.Reason.CrashIntoRobot);
            break;

            case "Destructable":
                BoxCollider boxCollider = other.gameObject.GetComponent<BoxCollider>();

                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

                boxCollider.isTrigger = true;

                Destroy(other.gameObject, 10f);
                break;

            default:
            break;
        }
    }
}

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

            default:
            break;
        }
    }
}

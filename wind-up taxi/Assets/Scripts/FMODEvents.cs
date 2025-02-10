using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }


    [field: Header("Car SFX")]

    [field: SerializeField] public EventReference carHit { get; private set; }


    [field: Header("Robot SFX")]

    [field: SerializeField] public EventReference robotDeathScream { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMODEvents instance in the scene.");
        }
        instance = this;
    }
}

using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference city { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference taxiMusic { get; private set; }



    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference carHit { get; private set; }
    [field: SerializeField] public EventReference carHonk { get; private set; }
    [field: SerializeField] public EventReference carEngine { get; private set; }
    [field: SerializeField] public EventReference carWindUp { get; private set; }


    [field: Header("NPC SFX")]
    [field: SerializeField] public EventReference robotDeathScream { get; private set; }
    [field: SerializeField] public EventReference npcHonk { get; private set; }


    [field: Header("UI")]
    [field: SerializeField] public EventReference pickUp { get; private set; }
    [field: SerializeField] public EventReference dropOff { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMODEvents instance in the scene.");
        }
        instance = this;
    }
}

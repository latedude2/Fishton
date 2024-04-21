using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FishCaught : NetworkBehaviour
{
    [SerializeField]
    private GameObject fish;

    [SerializeField]
    private bool activate = false;

    private EventManager Events { get; set; }


    private void Awake()
    {
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnFishingStateChanged;
    }

    void Update()
    {
        if (activate == true)
        {
            CatchFish(fish);

            activate = false;
        }
    }

    private void OnFishingStateChanged(FishEncounterState newState)
    {
        if (newState == FishEncounterState.Succeeeded)
        {
            CatchFish(fish);
        }
    }

    private void CatchFish(GameObject fish)
    {
        GameObject instancedFish = Instantiate(fish, new Vector3(0, 20, 0), Quaternion.identity);
        instancedFish.GetComponent<AnimateCaughtFish>().CatchingPlayer = gameObject;
    } 
}

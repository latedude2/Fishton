using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// DEPRECEATED
/// </summary>
public class FishCaught : NetworkBehaviour
{
    [SerializeField]
    private GameObject fish;

    private EventManager Events { get; set; }


    private void Awake()
    {
        //Events = EventManager.Get(gameObject);
        //Events.OnFishingStateChanged += OnFishingStateChanged;
    }

    private void OnFishingStateChanged(FishEncounterState newState)
    {
        if (newState == FishEncounterState.Succeeeded)
        {
            CatchFish();
        }
    }

    public void CatchFish()
    {
        GameObject instancedFish = Instantiate(fish, new Vector3(0, 20, 0), Quaternion.identity);
        instancedFish.GetComponent<AnimateCaughtFish>().CatchingPlayer = gameObject;
    } 
}

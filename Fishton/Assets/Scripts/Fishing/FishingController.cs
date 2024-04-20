using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    private FishEncounter CurrentEncounter = null;
    
    private void Awake()
    {
        EventManager.OnFishingStateChanged += (FishEncounterState NewState) => 
        {
            Debug.Log(NewState);
        };
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if(CurrentEncounter == null)
        {
            StartNewEncounter();
        }
    }

    private void StartNewEncounter()
    {
        CurrentEncounter = gameObject.AddComponent<FishEncounter>(); 
        CurrentEncounter.StartEncounter();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : LocalPlayerComponent
{
    private EventManager Events { get; set; }
    private FishEncounter CurrentEncounter = null;

    protected override void MyAwake()
    {
        base.MyAwake();
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += (FishEncounterState NewState) => 
        {
            Debug.Log(NewState);
        };
        Events.OnFishEncounterFinished += () => 
        {
            if(CurrentEncounter != null)
            {
                Destroy(CurrentEncounter);
                CurrentEncounter = null;
            }
        };
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        Debug.Log("Creating New Encounter");
        CurrentEncounter = gameObject.AddComponent<FishEncounter>(); 
        CurrentEncounter.StartEncounter(GetComponent<Player>());
    }
}

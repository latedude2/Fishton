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

            if (NewState == FishEncounterState.Hooked)
            {
                if (IsLocalPlayer)
                    GetComponentInChildren<FishAlert>().ShowWarning();
            }
            else
            {
                if (IsLocalPlayer)
                    GetComponentInChildren<FishAlert>().HideWarning();
            }
        };
        Events.OnFishEncounterFinished += () => 
        {
            if(CurrentEncounter != null)
            {
                //Moved the destroy method to after the OnFishEncounterFinished invoke
                //Destroy(CurrentEncounter);
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
        CurrentEncounter.StartEncounter();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEncounter : MonoBehaviour
{
    private EventManager Events { get; set; }
    public FishEncounterState CurrentState
    {
        get
        {
            return _CurrentState;
        }
        set
        {
            _CurrentState = value;
            Events.OnFishingStateChanged?.Invoke(value);

            if(value == FishEncounterState.Caught)
                Events.OnFishCaught?.Invoke();

            if((value & FishEncounterState.Finished) == value)
                HandleFinished();
        }
    }

    private FishEncounterState _CurrentState = FishEncounterState.None;

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
    }

    public void StartEncounter()
    {
        StartCoroutine("HandleEventLoop");
    }

    public IEnumerator HandleEventLoop()
    {
        CurrentState = FishEncounterState.Throwing;
        yield return new WaitForSeconds(0);
        CurrentState = FishEncounterState.Idle;
        yield return new WaitForSeconds(0);
        CurrentState = FishEncounterState.Hooked;

        WaitForHookedEventHandler HookHandler = new WaitForHookedEventHandler();
        yield return HookHandler;

        if(HookHandler.DidPlayerSucceed)
        {
            CurrentState = FishEncounterState.Caught;
            WaitForFishingMinigameFinished FishingMinigameEventHandler = new WaitForFishingMinigameFinished(Events);
            yield return FishingMinigameEventHandler;
            if(FishingMinigameEventHandler.DidWin)
                CurrentState = FishEncounterState.Succeeeded;
        }

        CurrentState = FishEncounterState.Failed;
    }

    private void HandleFinished()
    {
        Debug.Log("Fish Encounter Finished");
        Events.OnFishEncounterFinished?.Invoke();
    }
}

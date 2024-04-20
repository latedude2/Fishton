using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEncounter : MonoBehaviour
{
    public FishEncounterState CurrentState
    {
        get
        {
            return _CurrentState;
        }
        set
        {
            _CurrentState = value;
            EventManager.OnFishingStateChanged?.Invoke(value);

            if(value == FishEncounterState.Caught)
                EventManager.OnFishCaught?.Invoke();
        }
    }
    private FishEncounterState _CurrentState = FishEncounterState.None;

    public void StartEncounter()
    {
        StartCoroutine("HandleEventLoop");
    }

    public IEnumerator HandleEventLoop()
    {
        CurrentState = FishEncounterState.Throwing;
        yield return new WaitForSeconds(2);
        CurrentState = FishEncounterState.Idle;
        yield return new WaitForSeconds(5);
        CurrentState = FishEncounterState.Hooked;
        
        WaitForHookedEventHandler HookHandler = new WaitForHookedEventHandler();
        yield return HookHandler;

        if(HookHandler.DidPlayerSucceed)
        {
            CurrentState = FishEncounterState.Caught;
        }
        else
        {
            CurrentState = FishEncounterState.Failed;
        }
    }
}

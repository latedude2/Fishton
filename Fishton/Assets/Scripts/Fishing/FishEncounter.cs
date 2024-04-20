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
        yield return new WaitForSeconds(0.5f);
        CurrentState = FishEncounterState.Caught;
    }
}

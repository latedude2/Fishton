using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FishEncounter : MonoBehaviour
{
    Player _player;
    private EventManager Events { get; set; }
    public FishEncounterState CurrentState
    {
        get
        {
            return _player.currentState;
        }
        set
        {
            _player.currentState = value;
            Events.OnFishingStateChanged?.Invoke(value);

            if(value == FishEncounterState.Caught)
                Events.OnFishCaught?.Invoke();

            if((value & FishEncounterState.Finished) == value)
                HandleFinished();
        }
    }

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
    }

    public void StartEncounter(Player player)
    {
        _player = player;
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

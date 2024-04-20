using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        }
    }

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
    }

    public void StartEncounter(Player player)
    {
        _player = player;
        player.onFishEncounterChange += _OnFishEncounterChange;
        StartCoroutine("HandleEventLoop");
    }

    private void _OnFishEncounterChange(FishEncounterState previousState, FishEncounterState newState)
    {
        Events.OnFishingStateChanged?.Invoke(newState);

        if (newState == FishEncounterState.Caught)
            Events.OnFishCaught?.Invoke();

        if ((newState & FishEncounterState.Finished) == newState)
            HandleFinished();
    }

    public IEnumerator HandleEventLoop()
    {
        CurrentState = FishEncounterState.Throwing;
        yield return new WaitForSeconds(2);
        CurrentState = FishEncounterState.Idle;
        yield return new WaitForSeconds(2);
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

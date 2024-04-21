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
            /*
            Events.OnFishingStateChanged?.Invoke(value);

            if(value == FishEncounterState.Caught)
                Events.OnFishCaught?.Invoke();

            if((value & FishEncounterState.Finished) == value)
                StartCoroutine("HandleFinished");
                */
        }
    }

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
    }

    private void OnDestroy()
    {
        _player.onFishEncounterChange -= _OnFishEncounterChange;
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
        yield return new WaitForSeconds(1);
        CurrentState = FishEncounterState.Idle;
        yield return new WaitForSeconds(3);
        CurrentState = FishEncounterState.Hooked;

        WaitForHookedEventHandler HookHandler = new WaitForHookedEventHandler();
        yield return HookHandler;

        if(HookHandler.DidPlayerSucceed)
        {
            CurrentState = FishEncounterState.Caught;
            WaitForFishingMinigameFinished FishingMinigameEventHandler = new WaitForFishingMinigameFinished(Events);
            yield return FishingMinigameEventHandler;
            if(FishingMinigameEventHandler.DidWin)
            {
                CurrentState = FishEncounterState.Succeeeded;
                yield break;
            }
        }

        CurrentState = FishEncounterState.Failed;
    }

    private IEnumerator HandleFinished()
    {
        Debug.Log("Fish Encounter Finished");
        yield return new WaitForSeconds(3);
        Events.OnFishEncounterFinished?.Invoke();
    }
}

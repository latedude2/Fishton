using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameController : LocalPlayerComponent
{
    [SerializeField]
    private FishingMinigame MinigamePrefab;

    private EventManager Events;
    private FishingMinigame MinigameInstance;


    protected override void MyAwake()
    {
        base.MyAwake();
        DoInitialize();
    }

    private void DoInitialize()
    {
        Events = EventManager.Get(gameObject);
        Events.OnFishCaught += OnFishCaught;
    }

    private void OnFishCaught()
    {
        MinigameInstance = Instantiate(MinigamePrefab);
        MinigameInstance.OnGameFinished += OnGameFinished;
    }

    private void OnGameFinished(bool WonGame)
    {
        Destroy(MinigameInstance.gameObject);
        MinigameInstance = null;

        Events.OnFishingMinigameFinished.Invoke(WonGame);
    }
}

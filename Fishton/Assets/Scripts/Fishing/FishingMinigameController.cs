using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameController : LocalPlayerComponent
{
    [SerializeField]
    private FishingMinigame MinigamePrefab;

    private EventManager Events;
    private FishingMinigame MinigameInstance;

    public void OnEnable()
    {
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
    }
}

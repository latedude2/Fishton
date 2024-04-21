using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingMinigameController : LocalPlayerComponent
{
    [SerializeField]
    private FishingMinigame MinigamePrefab;

    private EventManager Events;
    private FishingMinigame MinigameInstance;

    [SerializeField]
    private bool CheatWinFive = false;


    [SerializeField]
    private GameObject fish;


    protected override void MyAwake()
    {
        base.MyAwake();
        DoInitialize();
    }

    private void Update()
    {
        if(CheatWinFive)
        {
            for(int i = 0; i < 5; i++)
                Events.OnFishingMinigameFinished.Invoke(true);

            CheatWinFive = false;
        }
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
        MinigameInstance.OnReelFishIn += OnReelFishIn;
    }

    private void OnReelFishIn()
    {
        GameObject instancedFish = Instantiate(fish, new Vector3(0, 20, 0), Quaternion.identity);
        instancedFish.GetComponent<AnimateCaughtFish>().CatchingPlayer = gameObject;
    }

    private void OnGameFinished(bool WonGame)
    {
        Destroy(MinigameInstance.gameObject);
        MinigameInstance = null;
        Events.OnFishingMinigameFinished.Invoke(WonGame);
    }
}

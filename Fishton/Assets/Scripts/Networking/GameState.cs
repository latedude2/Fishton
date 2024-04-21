using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public delegate void OnFishCounterUpdatedDelegate(int NewFishCount);

public class GameState : NetworkBehaviour
{
    public static GameState Instance { get; private set; }
    
    public OnFishCounterUpdatedDelegate OnFishCounterUpdated;

    public NetworkVariable<int> FishCaught = new NetworkVariable<int>(0);

    private void Awake()
    {
        Instance = this;

        FishCaught.OnValueChanged += FishValueChanged;
    }

    private void Start()
    {
        FishValueChanged(FishCaught.Value, FishCaught.Value);
    }

    public void Increment()
    {
        FishCaught.Value++;
    }

    private void FishValueChanged(int Old, int New)
    {
        OnFishCounterUpdated?.Invoke(New);
    }
}

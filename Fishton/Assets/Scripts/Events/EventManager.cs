using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnFishingStateChangedDelegate(FishEncounterState NewState);
public delegate void OnCaughtFishDelegate(/*TODO: Add fish data here*/);

public static class EventManager
{
    public static OnFishingStateChangedDelegate OnFishingStateChanged;
    public static OnCaughtFishDelegate OnFishCaught;
}

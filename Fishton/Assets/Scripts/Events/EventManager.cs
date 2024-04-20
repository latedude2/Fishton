using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnFishingStateChangedDelegate(FishEncounterState NewState);

public static class EventManager
{
    public static OnFishingStateChangedDelegate OnFishingStateChanged;
}

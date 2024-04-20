using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnFishingStateChangedDelegate(FishEncounterState NewState);
public delegate void OnCaughtFishDelegate(/*TODO: Add fish data here*/);
public delegate void OnFishEncounterFinishedDelegate();
public delegate void OnFishingMinigameFinished(bool DidSucceed);

public class EventManager : MonoBehaviour
{
    public static EventManager Get(GameObject Obj)
    {
        GameObject Root = GetRoot(Obj);
        var Manager = Root.GetComponent<EventManager>();
        if(Manager == null)
            Manager = Root.AddComponent<EventManager>();
        return Manager;
    }

    private static GameObject GetRoot(GameObject Obj)
    {
        Transform Parent = Obj.transform.parent;
        if(Parent == null)
            return Obj;

        return GetRoot(Parent.gameObject);
    }

    public OnFishingStateChangedDelegate OnFishingStateChanged;
    public OnCaughtFishDelegate OnFishCaught;
    public OnFishEncounterFinishedDelegate OnFishEncounterFinished;
    public OnFishingMinigameFinished OnFishingMinigameFinished;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingStateColourer : MonoBehaviour
{
    private EventManager Events { get; set; }
    private FishEncounterState LastSeenState;

    private void Awake()
    {
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnFishingStateChanged;
        Events.OnFishEncounterFinished += OnFishingCompleted;

        SetColor(Color.white);
    }

    private void OnFishingStateChanged(FishEncounterState NewState)
    {
        SetColor(GetColorFromState(NewState));
    }

    private Color GetColorFromState(FishEncounterState NewState)
    {
        LastSeenState = NewState;
        switch (NewState)
        {
            case FishEncounterState.Throwing:
                return Color.gray;
            case FishEncounterState.Idle:
                return Color.yellow;
            case FishEncounterState.Hooked:
                return new Color(1.0f, 165/255.0f, 0.0f);
            case FishEncounterState.Caught:
                return Color.green;
            case FishEncounterState.Failed:
                return Color.red;
            default:
                return Color.white;
        }
    }

    private void OnFishingCompleted()
    {
        StartCoroutine("ResetColor");
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(2);
        if (LastSeenState == (LastSeenState & FishEncounterState.Finished))
            SetColor(Color.white);
    }

    private void SetColor(Color NewColor)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = NewColor;
    }
}

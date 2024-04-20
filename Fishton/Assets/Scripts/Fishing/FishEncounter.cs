using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEncounter
{
    public FishEncounter()
    {
        CurrentState = FishEncounterState.Idle;
    }

    public FishEncounterState CurrentState { get; private set; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDefinition : ScriptableObject
{
    [Tooltip("The time the player has to catch the fish when it becomes hooked")]
    public float TimeToCatch = 0.2f;
}

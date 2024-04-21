using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;
    private EventManager Events { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("FishingState", 0);
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnFishingStateChanged;
    }

    private void OnFishingStateChanged(FishEncounterState NewState)
    {
        PlayAnimation(GetAnimationStateNumberFromState(NewState));
        if(NewState == FishEncounterState.Finished || NewState == FishEncounterState.Failed || NewState == FishEncounterState.Succeeeded)
        {
            Invoke(nameof(ResetAnimationToIdle), 0.5f);
        }
    }

    void ResetAnimationToIdle()
    {
        animator.SetInteger("FishingState", 0);
    }
    
    private int GetAnimationStateNumberFromState(FishEncounterState NewState)
    {
        switch (NewState)
        {
            case FishEncounterState.None:
                return 0;
            case FishEncounterState.Throwing:
                return 1;
            case FishEncounterState.Idle:
                return 2;
            case FishEncounterState.Hooked:
                return 3; //The animator will transition to the next animation state (4) automatically
            case FishEncounterState.Caught:
                return 4;
            case FishEncounterState.Failed:
                return 5;
            case FishEncounterState.Succeeeded:
                return 7;
            default:
                Debug.Log("DEFAULT in the player animation");
                return 0;
        }
    }

    void PlayAnimation(int state)
    {
        animator.SetInteger("FishingState", state);
    }


}

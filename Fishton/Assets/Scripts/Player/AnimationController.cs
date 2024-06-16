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
        animator.speed = 0.5f;
        Events = EventManager.Get(gameObject);
        Events.OnFishingStateChanged += OnFishingStateChanged;
    }

    private void OnFishingStateChanged(FishEncounterState NewState)
    {
        PlayAnimation(GetAnimationStateNumberFromState(NewState));
        if(NewState == FishEncounterState.Finished || NewState == FishEncounterState.Failed || NewState == FishEncounterState.Succeeeded)
        {
            Invoke(nameof(ResetAnimationToIdle), 1f);
        }
    }

    void ResetAnimationToIdle()
    {
        animator.speed = 0.5f;
        animator.SetInteger("FishingState", 0);
    }
    
    private int GetAnimationStateNumberFromState(FishEncounterState NewState)
    {
        switch (NewState)
        {
            case FishEncounterState.None:
                animator.speed = 0.2f;
                return 0;
            case FishEncounterState.Throwing:
                animator.speed = 1f;
                return 1;
            case FishEncounterState.Idle:
                animator.speed = 1f;
                return 2;
            case FishEncounterState.Hooked:
                animator.speed = 1f;
                return 3; //The animator will transition to the next animation state (4) automatically
            case FishEncounterState.Caught:
                animator.speed = 1f;
                return 4;
            case FishEncounterState.Failed:
                animator.speed = 1f;
                return 5;
            case FishEncounterState.Succeeeded:
                animator.speed = 1f;
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

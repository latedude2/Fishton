using UnityEngine;

public class WaitForFishingMinigameFinished : CustomYieldInstruction
{
    public WaitForFishingMinigameFinished(EventManager Events)
    {
        Events.OnFishingMinigameFinished += (bool Won) => {
            DidWin = Won;
            IsFinished = true;
        };
    }

    public bool DidWin { get; private set; }
    private bool IsFinished = false;

    public override bool keepWaiting => !IsFinished;
}
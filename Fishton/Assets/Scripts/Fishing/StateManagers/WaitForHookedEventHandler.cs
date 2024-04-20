using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForHookedEventHandler : CustomYieldInstruction
{
    public WaitForHookedEventHandler()
    {
        DidPlayerSucceed = false;
        StartTime = Time.time;
        TimeToWait = 0.6f;
    }

    public bool DidPlayerSucceed { get; private set; }

    public override bool keepWaiting {
        get 
        {
            if(Input.GetMouseButtonUp(0))
            {
                DidPlayerSucceed = true;
                return false;
            }
            return !(Time.time >= StartTime + TimeToWait);
        }
        
    }

    private float TimeToWait;
    private float StartTime;
}
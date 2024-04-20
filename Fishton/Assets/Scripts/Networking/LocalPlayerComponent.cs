using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LocalPlayerComponent : NetworkBehaviour
{
    private void Awake()
    {
        enabled = IsLocalPlayer;
        Debug.Log($"is local player : {IsLocalPlayer}");
        if (enabled)
        {
            MyAwake();
        }
    }

    protected virtual void MyAwake()
    {

    }
}

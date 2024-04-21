using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LocalPlayerComponent : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        enabled = IsLocalPlayer;

        if (enabled)
        {
            MyAwake();
        }
    }

    protected virtual void MyAwake()
    {

    }
}

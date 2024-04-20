using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LocalPlayerComponent : NetworkBehaviour
{
    private void Awake()
    {
        // TODO: Enable when we want to do networking
        // enabled = IsLocalPlayer;
    }
}

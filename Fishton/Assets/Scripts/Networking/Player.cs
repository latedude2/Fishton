using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{

    public NetworkVariable<int> positionIndex = new NetworkVariable<int>(0);

    public void Start()
    {
        positionIndex.OnValueChanged += OnPositionIndexChanged;
    }

    public override void OnNetworkSpawn()
    {
        if(IsHost)
        {
            positionIndex.Value = PlayerPositioning.GetFirstAvailableSpawnPoint();
            PlayerPositioning.GetSpawnPoint(positionIndex.Value).isOccupied = true;
        }
        
    }

    void OnPositionIndexChanged(int oldIndex, int newIndex)
    {
        transform.position = PlayerPositioning.instance.SpawnPoints[newIndex].transform.position;
        transform.rotation = PlayerPositioning.instance.SpawnPoints[newIndex].transform.rotation;
    }

}

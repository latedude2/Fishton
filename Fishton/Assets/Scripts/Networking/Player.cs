using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{

    public NetworkVariable<int> positionIndex = new NetworkVariable<int>(0);

    public void Awake()
    {
        positionIndex.OnValueChanged += OnPositionIndexChanged;
    }



    public override void OnNetworkSpawn()
    {
        Debug.Log("Player spawned");
        if(IsHost)
        {
            positionIndex.Value = PlayerPositioning.GetFirstAvailableSpawnPoint();
            PlayerPositioning.GetSpawnPoint(positionIndex.Value).isOccupied = true;
            transform.position = PlayerPositioning.instance.SpawnPoints[positionIndex.Value].transform.position;
            transform.rotation = PlayerPositioning.instance.SpawnPoints[positionIndex.Value].transform.rotation;
        }
        
    }

    void OnPositionIndexChanged(int oldIndex, int newIndex)
    {
        transform.position = PlayerPositioning.instance.SpawnPoints[newIndex].transform.position;
        transform.rotation = PlayerPositioning.instance.SpawnPoints[newIndex].transform.rotation;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Player : NetworkBehaviour
{

    public NetworkVariable<FishEncounterState> _CurrentState = new NetworkVariable<FishEncounterState>(value: FishEncounterState.None, readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);


    public NetworkVariable<int> positionIndex = new NetworkVariable<int>(0);

    public delegate void FishEncounterChangeHandler(FishEncounterState previousState, FishEncounterState newState);
    public FishEncounterChangeHandler onFishEncounterChange { get; set; }

    public FishEncounterState currentState { get => _CurrentState.Value; set { _CurrentState.Value = value; } }

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
        _CurrentState.OnValueChanged += OnStateChanged;
    }

    private void OnStateChanged(FishEncounterState previousValue, FishEncounterState newValue)
    {
        onFishEncounterChange(previousValue, newValue);
    }

    public override void OnDestroy()
    {
        PlayerPositioning.GetSpawnPoint(positionIndex.Value).isOccupied = false;
    }

    void OnPositionIndexChanged(int oldIndex, int newIndex)
    {
        transform.position = PlayerPositioning.instance.SpawnPoints[newIndex].transform.position;
        transform.rotation = PlayerPositioning.instance.SpawnPoints[newIndex].transform.rotation;
    }

}

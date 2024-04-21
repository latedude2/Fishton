using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class Player : NetworkBehaviour
{
    public NetworkVariable<FishEncounterState> networkedFishingState = new NetworkVariable<FishEncounterState>(value: FishEncounterState.None, readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);


    public NetworkVariable<int> positionIndex = new NetworkVariable<int>(0);

    [SerializeField]
    private GameObject LocalIndicator;

    public void Awake()
    {
        positionIndex.OnValueChanged += OnPositionIndexChanged;
        if (IsLocalPlayer == false)
        {
            GetComponent<EventManager>().OnFishingStateChanged += ((newState) => { 
                networkedFishingState.Value = newState;
            });
        }
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

        if (IsLocalPlayer == false)
        {
            networkedFishingState.OnValueChanged += OnStateChanged;
        }
        else
        {
            LocalIndicator.SetActive(true);
        }
        if(IsHost)
        {
            networkedFishingState.OnValueChanged += (FishEncounterState OldValue, FishEncounterState NewValue) => 
            {
                if(NewValue == FishEncounterState.Succeeeded)
                    GameState.Instance.Increment();
            };
        }
    }

    private void OnStateChanged(FishEncounterState previousValue, FishEncounterState newValue)
    {
        GetComponent<EventManager>().OnFishingStateChanged?.Invoke(newValue);
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

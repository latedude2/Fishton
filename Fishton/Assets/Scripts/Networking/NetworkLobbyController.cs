using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport.Relay;
using Unity.Networking.Transport;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
using Unity.Services.Core;

public class NetworkLobbyController
{
    private string _playerID = "Not signed in";
    string _joinCode = "n/a";

    string autoSelectRegionName = "auto-select (QoS)";
    int regionAutoSelectIndex = 0;
    List<Region> regions = new List<Region>();
    List<string> regionOptions = new List<string>();
    string hostLatestMessageReceived;
    string playerLatestMessageReceived;

    // Allocation response objects
    Allocation hostAllocation;
    JoinAllocation playerAllocation;

    // Control vars
    bool isHost;
    bool isPlayer;

    NetworkDriver hostDriver;
    NetworkDriver playerDriver;
    NativeList<NetworkConnection> serverConnections;
    NetworkConnection clientConnection;


    public string playerID => _playerID;
    public string joinCode => _joinCode;

    public NetworkLobbyController()
    {
        StartUnityServive();
    }

    public async void StartUnityServive()
    {
        // Initialize Unity Services
        await UnityServices.InitializeAsync();
        await _SignIn();
    }

    public async Task StartHostProcess()
    {
        //await _SignIn();

        await _Region();

        await _AllocateRelay();

        _BindHost();

        await _GetJoinCode();

        isHost = true;
    }
    public void StopHostProcess()
    {
        isHost = false;
    }

    public async Task StartClientProcess(string sessionCode)
    {
        //await _SignIn();

        await Task.Delay(1000);

        await _Join(sessionCode);

        _BindClient();

        _ConnectClient();
    }


    private async Task _SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        _playerID = AuthenticationService.Instance.PlayerId;
        Debug.Log($"Signed in. Player ID: {_playerID}");
    }

    private async Task _Join(string sessionCode)
    {
        // Input join code in the respective input field first.
        if (String.IsNullOrEmpty(sessionCode))
        {
            Debug.LogError("Please input a join code.");
            return;
        }

        Debug.Log("Player - Joining host allocation using join code. Upon success, I have 10 seconds to BIND to the Relay server that I've allocated.");
        Debug.Log($"the session code is {sessionCode}");
        try
        {
            playerAllocation = await RelayService.Instance.JoinAllocationAsync(sessionCode);
            Debug.Log("Player Allocation ID: " + playerAllocation.AllocationId);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }

        Debug.Log($"The region is {playerAllocation?.Region}");
        Debug.Log($"The region is ???");

    }


    #region Host
    private async Task _Region()
    {
        Debug.Log("Host - Getting regions.");
        var allRegions = await RelayService.Instance.ListRegionsAsync();
        regions.Clear();
        regionOptions.Clear();
        foreach (var region in allRegions)
        {
            Debug.Log(region.Id + ": " + region.Description);
            regionOptions.Add(region.Id);
            regions.Add(region);
        }
    }

    private async Task _AllocateRelay()
    {
        Debug.Log("Host - Creating an allocation. Upon success, I have 10 seconds to BIND to the Relay server that I've allocated.");

        // 0 should be the default region index
        string region = "europe-north1";// regions[1].Id;
        Debug.Log($"The chosen region is: {region ?? autoSelectRegionName}");

        // Set max connections. Can be up to 100, but note the more players connected, the higher the bandwidth/latency impact.
        int maxConnections = 100;

        // Important: Once the allocation is created, you have ten seconds to BIND, else the allocation times out.
        hostAllocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        Debug.Log($"Host Allocation ID: {hostAllocation.AllocationId}, region: {hostAllocation.Region}");

        // Initialize NetworkConnection list for the server (Host).
        // This list object manages the NetworkConnections which represent connected players.
        serverConnections = new NativeList<NetworkConnection>(maxConnections, Allocator.Persistent);
    }

    private void _BindHost()
    {
        Debug.Log("Host - Binding to the Relay server using UTP.");

        // Extract the Relay server data from the Allocation response.
        var relayServerData = new RelayServerData(hostAllocation, "udp");

        // Create NetworkSettings using the Relay server data.
        var settings = new NetworkSettings();
        settings.WithRelayParameters(ref relayServerData);

        // Create the Host's NetworkDriver from the NetworkSettings.
        hostDriver = NetworkDriver.Create(settings);

        // Bind to the Relay server.
        if (hostDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
        {
            Debug.LogError("Host client failed to bind");
        }
        else
        {
            if (hostDriver.Listen() != 0)
            {
                Debug.LogError("Host client failed to listen");
            }
            else
            {
                Debug.Log("Host client bound to Relay server");
            }
        }      
    }


    private async Task _GetJoinCode()
    {
        Debug.Log("Host - Getting a join code for my allocation. I would share that join code with the other players so they can join my session.");

        try
        {
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);
            Debug.Log("Host - Got join code: " + joinCode);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }

    #endregion Host


    #region Client

    private void _BindClient()
    {
        Debug.Log("Player - Binding to the Relay server using UTP.");

        // Extract the Relay server data from the Join Allocation response.
        var relayServerData = new RelayServerData(playerAllocation, "udp");

        // Create NetworkSettings using the Relay server data.
        var settings = new NetworkSettings();
        settings.WithRelayParameters(ref relayServerData);

        // Create the Player's NetworkDriver from the NetworkSettings object.
        playerDriver = NetworkDriver.Create(settings);

        // Bind to the Relay server.
        if (playerDriver.Bind(NetworkEndPoint.AnyIpv4) != 0)
        {
            Debug.LogError("Player client failed to bind");
        }
        else
        {
            Debug.Log("Player client bound to Relay server");
        }
    }

    private void _ConnectClient()
    {
        Debug.Log("Player - Connecting to Host's client.");

        // Sends a connection request to the Host Player.
        clientConnection = playerDriver.Connect();
    }

    #endregion Client
}

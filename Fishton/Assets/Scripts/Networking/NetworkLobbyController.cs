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

public class NetworkLobbyController
{
    private string _playerID = "Not signed in";
    string playerId = "Not signed in";
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

    public NetworkLobbyController()
    {

    }

    public async void StartHostProcess()
    {
        //Sign in
        _SignIn();

        //Get regions
        _GetRegions();

        //Allocate relay
        _AllocateRelay();
    }

    private async void _SignIn()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        _playerID = AuthenticationService.Instance.PlayerId;
        Debug.Log($"Signed in. Player ID: {_playerID}");
    }

    private async void _GetRegions()
    {
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

    private async void _AllocateRelay()
    {
        Debug.Log("Host - Creating an allocation. Upon success, I have 10 seconds to BIND to the Relay server that I've allocated.");

        // Determine region to use (user-selected or auto-select/QoS)
        string region = regions[0].Id;
        Debug.Log($"The chosen region is: {region ?? autoSelectRegionName}");

        // Set max connections. Can be up to 100, but note the more players connected, the higher the bandwidth/latency impact.
        int maxConnections = 4;

        // Important: Once the allocation is created, you have ten seconds to BIND, else the allocation times out.
        hostAllocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        Debug.Log($"Host Allocation ID: {hostAllocation.AllocationId}, region: {hostAllocation.Region}");

        // Initialize NetworkConnection list for the server (Host).
        // This list object manages the NetworkConnections which represent connected players.
        serverConnections = new NativeList<NetworkConnection>(maxConnections, Allocator.Persistent);
    }

    private async void _BindHost()
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
}

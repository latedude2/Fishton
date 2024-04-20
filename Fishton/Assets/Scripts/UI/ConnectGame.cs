using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay.Models;
using System;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity;


public class ConnectGame : MonoBehaviour
{
    [SerializeField] Button _joinButton;
    [SerializeField] Button _hostButton;
    [SerializeField] Button _characterCreatorButton;
    [SerializeField] Button _quitButton;
    [SerializeField] Button _playButton;
    [SerializeField] GameObject _sessionCodeContainer;
    [SerializeField] TMP_InputField _sessionCodeInputField;
    [SerializeField] TMP_Text _sessionCodeText;

    private void Start()
    {
        //Set initial states
        _sessionCodeContainer.gameObject.SetActive(false);
        _sessionCodeText.gameObject.SetActive(false);

        //Assign actions to ui elements
        _joinButton.onClick.AddListener(() => { _ButtonPressed_Join(); });
        _hostButton.onClick.AddListener(() => { _ButtonPressed_Host(); });
        _playButton.onClick.AddListener(() => { _ButtonPressed_Play(); });
    }

    private void _ButtonPressed_Join()
    {
        _ShowSessionCodeInput();
        _HideSessionCode();
    }

    private void _ButtonPressed_Play()
    {
        string sessionCode = _sessionCodeInputField.text;
        if (String.IsNullOrEmpty(sessionCode))
        {
            Debug.LogError("Please input a join code.");
            return;
        }
        StartClientWithRelay(sessionCode);

    }

    private void _ButtonPressed_Host()
    {
        _HideSessionCodeInput();
        _ShowSessionCode();
        StartHostWithRelay();
    }

    
    #region Control UI
    private void _ShowSessionCodeInput()
    {
        _sessionCodeContainer.gameObject.SetActive(true);
    }

    private void _HideSessionCodeInput()
    {
        _sessionCodeContainer.gameObject.SetActive(false);
    }

    private void _ShowSessionCode()
    {
        _sessionCodeText.gameObject.SetActive(true);
    }

    private void _HideSessionCode()
    {
        _sessionCodeText.gameObject.SetActive(false);
    }
    #endregion Control UI

    public async Task<string> StartHostWithRelay(int maxConnections = 100)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        //Set relay server data

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "utp"));
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
    



}

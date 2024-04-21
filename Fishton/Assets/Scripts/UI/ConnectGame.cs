using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;

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
    [SerializeField] TMP_Text _sessionCodeTextInGame;

    public AudioSource _audioSource;

    [SerializeField] List<GameObject> _thingsToHide = new List<GameObject>();

    private void Start()
    {
        //Set initial states
        //_sessionCodeContainer.gameObject.SetActive(false);
        //_sessionCodeText.gameObject.SetActive(false);

        //Assign actions to ui elements
        _joinButton.onClick.AddListener(() => { _ButtonPressed_Join(); });
        _hostButton.onClick.AddListener(() => { _ButtonPressed_Host(); });
        _playButton.onClick.AddListener(() => { _ButtonPressed_Play(); });
        _quitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void _ButtonPressed_Join()
    {
        /*_ShowSessionCodeInput();
        _HideSessionCode();*/
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
        //GetComponent<MainMenuController>().AnimateOut();
        
    }

    private void _ButtonPressed_Host()
    {
        Debug.Log("Host button pressed");
        //_HideSessionCodeInput();
        //_ShowSessionCode();
        StartHostWithRelay();
        //GetComponent<MainMenuController>().AnimateOut();
        HideMainMenuUI();
    }

    void HideMainMenuUI()
    {
        foreach (GameObject item in _thingsToHide)
        {
            item.SetActive(false);
        }
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
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, "europe-north1");
        //Set relay server data
        #if UNITY_WEBGL
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(new RelayServerData(allocation, "wss"));
        unityTransport.UseWebSockets = true;
        #endif
        #if !UNITY_WEBGL
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
        #endif
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        Debug.Log($"Join code: {joinCode}");
        _sessionCodeText.text = joinCode;
        _sessionCodeTextInGame.text = joinCode;
        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode)
    {
        await UnityServices.InitializeAsync();
        Debug.Log("Starting client with relay");
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in anonymously");
        }

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        Debug.Log($"Join allocation: {joinAllocation}");
        #if UNITY_WEBGL
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(new RelayServerData(joinAllocation, "wss"));
        unityTransport.UseWebSockets = true;
        #endif
        #if !UNITY_WEBGL
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        #endif
        Debug.Log("Starting client");
        _audioSource.Play();
        bool startedSuccessfully = NetworkManager.Singleton.StartClient();
        if (startedSuccessfully)
            HideMainMenuUI();
        else
        {
            ShowWrongCodeWarning();
        }
        return !string.IsNullOrEmpty(joinCode) && startedSuccessfully;
    }

    private void ShowWrongCodeWarning()
    {
        Debug.Log("Wrong logging00");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _joinButton;
    [SerializeField] Button _hostButton;
    [SerializeField] Button _characterCreatorButton;
    [SerializeField] Button _quitButton;
    [SerializeField] Button _playButton;
    [SerializeField] GameObject _sessionCodeContainer;
    [SerializeField] TMP_InputField _sessionCodeInputField;
    [SerializeField] TMP_Text _sessionCodeText;

    NetworkLobbyController _lobbyController;

    IEnumerator _hostSetUpRoutine;
    IEnumerator _clientSetUpRoutine;

    private void Awake()
    {
        _lobbyController = new NetworkLobbyController();
    }

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
        _StopHostProcess();
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
        _StartClientProcess(sessionCode);
    }

    private void _ButtonPressed_Host()
    {
        _HideSessionCodeInput();
        _ShowSessionCode();
        _StartHostProcess();
    }

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


    private void _StartHostProcess()
    {
        _hostSetUpRoutine = _Co_StartHost();
        StartCoroutine(_hostSetUpRoutine);
    }
    private void _StopHostProcess()
    {
        
    }

    IEnumerator _Co_StartHost()
    {
        Task task = _lobbyController.StartHostProcess();
        yield return new WaitUntil(()=> task.IsCompleted);

        _AssignJoinCode(_lobbyController.joinCode);
    }

    private void _AssignJoinCode(string joinCode)
    {
        _sessionCodeText.text = joinCode;
    }


    private void _StartClientProcess(string sessionCode)
    {
        _clientSetUpRoutine = _Co_StartClient(sessionCode);
        StartCoroutine(_clientSetUpRoutine);
    }
    private void _StopClientProcess()
    {

    }

    IEnumerator _Co_StartClient(string sessionCode)
    {
        Task task = _lobbyController.StartClientProcess(sessionCode);
        yield return new WaitUntil(() => task.IsCompleted);

        Debug.Log("Client is done !");
    }
}

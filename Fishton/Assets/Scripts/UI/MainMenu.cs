using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _joinButton;
    [SerializeField] Button _hostButton;
    [SerializeField] Button _characterCreatorButton;
    [SerializeField] Button _quitButton;
    [SerializeField] TMP_InputField _sessionCodeInputField;
    [SerializeField] TMP_Text _sessionCodeText;

    NetworkLobbyController _lobbyController;

    private void Awake()
    {
        _lobbyController = new NetworkLobbyController();
    }

    private void Start()
    {
        //Set initial states
        _sessionCodeInputField.gameObject.SetActive(false);
        _sessionCodeText.gameObject.SetActive(false);

        //Assign actions to ui elements
        _joinButton.onClick.AddListener(() => { _ButtonPressed_Join(); });
        _hostButton.onClick.AddListener(() => { _ButtonPressed_Host(); });
    }

    private void _ButtonPressed_Join()
    {
        _StopHostGame();
        _StartJoinGame();
    }

    private void _ButtonPressed_Host()
    {
        _StopJoinGame();
        _StartHostGame();
    }


    private void _StartJoinGame()
    {
        _sessionCodeInputField.gameObject.SetActive(true);
    }
    private void _StopJoinGame()
    {
        _sessionCodeInputField.gameObject.SetActive(false);
    }

    private void _StartHostGame()
    {
        _sessionCodeText.gameObject.SetActive(true);

        _lobbyController.StartHostProcess();        
    }

    

    private void _StopHostGame()
    {
        _sessionCodeText.gameObject.SetActive(false);
    }
}

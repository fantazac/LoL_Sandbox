using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    private int characterInfoReceived;

    public delegate void OnConnectedToServerHandler();
    public event OnConnectedToServerHandler OnConnectedToServer;

    private void Start()
    {
        MainMenuManager mainMenuManager = GetComponent<MainMenuManager>();
        mainMenuManager.OnConnectingToServer += OnConnectingToServer;
    }

    private void OnConnectingToServer()
    {
        Connect();
    }

    private void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("MOBA v1.0.0");
    }

    private void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    private void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length > 1)
        {
            characterInfoReceived = PhotonNetwork.playerList.Length - 1;
            StartCoroutine(LoadPlayers());
        }
        else
        {
            OnConnectedToServer();
        }
    }

    private IEnumerator LoadPlayers()
    {
        yield return null;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Character"))
        {
            Character character = player.GetComponent<Character>();
            character.SendToServer_ConnectionInfoRequest();
            character.OnConnectionInfoReceived += OnConnectionInfoReceived;
        }
    }

    private void OnConnectionInfoReceived(Character character)
    {
        character.OnConnectionInfoReceived -= OnConnectionInfoReceived;
        if(--characterInfoReceived == 0)
        {
            OnConnectedToServer();
        }
    }
}

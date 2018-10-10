using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    private float loadingPlayersTime;
    private WaitForSeconds delayLoadingPlayers;

    public delegate void OnConnectedToServerHandler();
    public event OnConnectedToServerHandler OnConnectedToServer;

    private NetworkManager()
    {
        loadingPlayersTime = 1;
        delayLoadingPlayers = new WaitForSeconds(loadingPlayersTime);
    }

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

        foreach (Character character in FindObjectsOfType<Character>())
        {
            character.SendToServer_ConnectionInfoRequest();
        }

        yield return delayLoadingPlayers;

        OnConnectedToServer();
    }
}

using UnityEngine;
using System.Collections;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private readonly WaitForSeconds delayLoadingPlayers;

    public delegate void OnConnectedToServerHandler();

    public event OnConnectedToServerHandler OnConnectedToServer;

    private NetworkManager()
    {
        delayLoadingPlayers = new WaitForSeconds(1);
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
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            StartCoroutine(LoadPlayers());
        }
        else
        {
            OnConnectedToServer?.Invoke();
        }
    }

    private IEnumerator LoadPlayers()
    {
        yield return null;

        foreach (Champion champion in FindObjectsOfType<Champion>())
        {
            champion.SendToServer_ConnectionInfoRequest();
        }

        yield return delayLoadingPlayers;

        OnConnectedToServer?.Invoke();
    }
}

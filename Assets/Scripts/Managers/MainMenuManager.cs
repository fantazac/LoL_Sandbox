using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCamera;

    [SerializeField]
    private GameObject characterParent;

    private MainMenuState state;

    public delegate void OnConnectingToServerHandler();
    public event OnConnectingToServerHandler OnConnectingToServer;

    private void Start()
    {
        NetworkManager networkManager = GetComponent<NetworkManager>();
        networkManager.OnConnectedToServer += OnConnectedToServer;

        state = MainMenuState.MAIN;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == MainMenuState.CONNECTING || state == MainMenuState.TEAM_SELECT)
            {
                if (StaticObjects.OnlineMode)
                {
                    PhotonNetwork.Disconnect();
                    StaticObjects.OnlineMode = false;
                }
                state = MainMenuState.MAIN;
            }
            else if (state == MainMenuState.CHARACTER_SELECT)
            {
                if (StaticObjects.OnlineMode)
                {
                    state = MainMenuState.TEAM_SELECT;
                }
                else
                {
                    state = MainMenuState.MAIN;
                }
            }
            else if (state == MainMenuState.ON_HOLD)
            {
                mainMenuCamera.SetActive(true);
                StaticObjects.Character.CharacterMovement.UnsubscribeCameraEvent();
                PhotonNetwork.Destroy(StaticObjects.Character.transform.parent.gameObject);
                StaticObjects.Character = null;
                StaticObjects.CharacterCamera = null;
                state = MainMenuState.CHARACTER_SELECT;
            }
        }
    }

    private void OnGUI()
    {
        switch (state)
        {
            case MainMenuState.MAIN:
                if (GUILayout.Button("Online", GUILayout.Height(40)))
                {
                    state = MainMenuState.CONNECTING;
                    StaticObjects.OnlineMode = true;
                    OnConnectingToServer();
                }
                if (GUILayout.Button("Offline", GUILayout.Height(40)))
                {
                    state = MainMenuState.CHARACTER_SELECT;
                    StaticObjects.OnlineMode = false;
                }
                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
                break;
            case MainMenuState.TEAM_SELECT:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length);
                }
                if (GUILayout.Button("BLUE", GUILayout.Height(40)))
                {
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
                }
                if (GUILayout.Button("RED", GUILayout.Height(40)))
                {
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.player.SetTeam(PunTeams.Team.red);
                }
                break;
            case MainMenuState.CHARACTER_SELECT:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + " - Team: " + PhotonNetwork.player.GetTeam());
                }
                if (GUILayout.Button("Ezreal", GUILayout.Height(40)))
                {
                    SpawnCharacter("Ezreal");
                }
                if (GUILayout.Button("Lucian", GUILayout.Height(40)))
                {
                    SpawnCharacter("Lucian");
                }
                break;
            case MainMenuState.ON_HOLD:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + " - Team: " + PhotonNetwork.player.GetTeam());
                }
                break;
        }
    }

    private void OnConnectedToServer()
    {
        state = MainMenuState.TEAM_SELECT;
    }

    private void SpawnCharacter(string characterName)
    {
        state = MainMenuState.ON_HOLD;

        GameObject characterTemplate = (GameObject)Instantiate(characterParent, new Vector3(), new Quaternion());
        GameObject character;
        if (StaticObjects.OnlineMode)
        {
            character = PhotonNetwork.Instantiate(characterName, Vector3.up * 0.5f, new Quaternion(), 0);
        }
        else
        {
            character = (GameObject)Instantiate(Resources.Load(characterName), Vector3.up * 0.5f, new Quaternion());
        }
        character.transform.parent = characterTemplate.transform;
        StaticObjects.Character = character.GetComponent<Character>();
        StaticObjects.CharacterCamera = characterTemplate.GetComponentInChildren<Camera>(true);
        StaticObjects.CharacterCamera.gameObject.SetActive(true);

        character.GetComponent<CharacterInput>().enabled = true;
        character.GetComponent<CharacterMouseManager>().enabled = true;

        mainMenuCamera.SetActive(false);
    }
}

enum MainMenuState
{
    MAIN,
    CONNECTING,
    TEAM_SELECT,
    CHARACTER_SELECT,
    ON_HOLD,
}

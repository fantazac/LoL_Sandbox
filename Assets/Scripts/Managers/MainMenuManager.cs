using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCamera;

    private readonly string characterParentPrefabPath;
    private GameObject characterParentPrefab;

    private MainMenuState state;

    private readonly Vector3 blueSpawn;
    private readonly Vector3 redSpawn;
    private readonly Vector3 offlineSpawn;
    private Vector3 selectedSpawn;

    public delegate void OnConnectingToServerHandler();
    public event OnConnectingToServerHandler OnConnectingToServer;

    private MainMenuManager()
    {
        blueSpawn = Vector3.left * 9 + Vector3.up * 0.5f + Vector3.back * 14;
        redSpawn = Vector3.right * 21 + Vector3.up * 0.5f + Vector3.forward * 9;
        offlineSpawn = Vector3.up * 0.5f;

        characterParentPrefabPath = "CharacterTemplatePrefab/CharacterTemplate";
    }

    private void Start()
    {
        NetworkManager networkManager = GetComponent<NetworkManager>();
        networkManager.OnConnectedToServer += OnConnectedToServer;

        state = MainMenuState.MAIN;

        LoadPrefabs();
    }

    private void LoadPrefabs()
    {
        characterParentPrefab = Resources.Load<GameObject>(characterParentPrefabPath);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        switch (state)
        {
            case MainMenuState.CONNECTING:
            case MainMenuState.TEAM_SELECT:
            {
                if (StaticObjects.OnlineMode)
                {
                    PhotonNetwork.Disconnect();
                    StaticObjects.OnlineMode = false;
                }

                state = MainMenuState.MAIN;
                break;
            }
            case MainMenuState.CHARACTER_SELECT when StaticObjects.OnlineMode:
                PhotonNetwork.LocalPlayer.LeaveCurrentTeam();
                state = MainMenuState.TEAM_SELECT;
                break;
            case MainMenuState.CHARACTER_SELECT:
                state = MainMenuState.MAIN;
                break;
            case MainMenuState.ON_HOLD:
            {
                mainMenuCamera.SetActive(true);
                StaticObjects.Champion.ChampionMovementManager.UnsubscribeCameraEvent();
                if (StaticObjects.OnlineMode)
                {
                    PhotonNetwork.Destroy(StaticObjects.Champion.transform.parent.gameObject);
                }
                else
                {
                    Destroy(StaticObjects.Champion.transform.parent.gameObject);
                }

                StaticObjects.Champion = null;
                StaticObjects.ChampionCamera = null;
                state = MainMenuState.CHARACTER_SELECT;
                break;
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
                    StaticObjects.Units = new Dictionary<string, Unit>();
                    OnConnectingToServer?.Invoke();
                }

                if (GUILayout.Button("Offline", GUILayout.Height(40)))
                {
                    selectedSpawn = offlineSpawn;
                    state = MainMenuState.CHARACTER_SELECT;
                    StaticObjects.OnlineMode = false;
                    StaticObjects.Units = new Dictionary<string, Unit>();
                }

                break;
            case MainMenuState.CONNECTING:
                GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
                break;
            case MainMenuState.TEAM_SELECT:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing() + "  -  Players Online: " + PhotonNetwork.PlayerList.Length);
                }

                if (GUILayout.Button("BLUE", GUILayout.Height(40)))
                {
                    selectedSpawn = blueSpawn;
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.LocalPlayer.JoinTeam(1);
                }

                if (GUILayout.Button("RED", GUILayout.Height(40)))
                {
                    selectedSpawn = redSpawn;
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.LocalPlayer.JoinTeam(2);
                }

                break;
            case MainMenuState.CHARACTER_SELECT:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing() + "  -  Players Online: " + PhotonNetwork.PlayerList.Length + " - Team: " +
                                    PhotonNetwork.LocalPlayer.GetPhotonTeam());
                }

                if (GUILayout.Button("CC", GUILayout.Height(40)))
                {
                    SpawnCharacter("CC");
                }

                if (GUILayout.Button("Ezreal", GUILayout.Height(40)))
                {
                    SpawnCharacter("Ezreal");
                }

                if (GUILayout.Button("Lucian", GUILayout.Height(40)))
                {
                    SpawnCharacter("Lucian");
                }

                if (GUILayout.Button("Miss Fortune", GUILayout.Height(40)))
                {
                    SpawnCharacter("MissFortune");
                }

                if (GUILayout.Button("Tristana", GUILayout.Height(40)))
                {
                    SpawnCharacter("Tristana");
                }

                if (GUILayout.Button("Varus", GUILayout.Height(40)))
                {
                    SpawnCharacter("Varus");
                }

                break;
            case MainMenuState.ON_HOLD:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing() + "  -  Players Online: " + PhotonNetwork.PlayerList.Length + " - Team: " +
                                    PhotonNetwork.LocalPlayer.GetPhotonTeam());
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

        GameObject characterTemplate = Instantiate(characterParentPrefab);
        GameObject champion;
        if (StaticObjects.OnlineMode)
        {
            champion = PhotonNetwork.Instantiate(characterName, selectedSpawn, new Quaternion());
        }
        else
        {
            champion = (GameObject)Instantiate(Resources.Load(characterName), selectedSpawn, new Quaternion());
        }

        champion.transform.parent = characterTemplate.transform;
        StaticObjects.Champion = champion.GetComponent<Champion>();
        StaticObjects.ChampionCamera = characterTemplate.GetComponentInChildren<Camera>();

        champion.GetComponent<InputManager>().enabled = true;
        champion.GetComponent<MouseManager>().enabled = true;

        mainMenuCamera.SetActive(false);
    }
}

public enum MainMenuState
{
    MAIN,
    CONNECTING,
    TEAM_SELECT,
    CHARACTER_SELECT,
    ON_HOLD,
}

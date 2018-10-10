using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuCamera;

    private string characterParentPrefabPath;
    private GameObject characterParentPrefab;

    private MainMenuState state;

    private Vector3 blueSpawn;
    private Vector3 redSpawn;
    private Vector3 offlineSpawn;
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
                if (StaticObjects.OnlineMode)
                {
                    PhotonNetwork.Destroy(StaticObjects.Character.transform.parent.gameObject);
                }
                else
                {
                    Destroy(StaticObjects.Character.transform.parent.gameObject);
                }
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
                    selectedSpawn = offlineSpawn;
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
                    selectedSpawn = blueSpawn;
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
                }
                if (GUILayout.Button("RED", GUILayout.Height(40)))
                {
                    selectedSpawn = redSpawn;
                    state = MainMenuState.CHARACTER_SELECT;
                    PhotonNetwork.player.SetTeam(PunTeams.Team.red);
                }
                break;
            case MainMenuState.CHARACTER_SELECT:
                if (StaticObjects.OnlineMode)
                {
                    GUILayout.Label("Ping: " + PhotonNetwork.GetPing().ToString() + "  -  Players Online: " + PhotonNetwork.playerList.Length + " - Team: " + PhotonNetwork.player.GetTeam());
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

        GameObject characterTemplate = Instantiate(characterParentPrefab);
        GameObject character;
        if (StaticObjects.OnlineMode)
        {
            character = PhotonNetwork.Instantiate(characterName, selectedSpawn, new Quaternion(), 0);
        }
        else
        {
            character = (GameObject)Instantiate(Resources.Load(characterName), selectedSpawn, new Quaternion());
        }
        character.transform.parent = characterTemplate.transform;
        StaticObjects.Character = character.GetComponent<Character>();
        StaticObjects.CharacterCamera = characterTemplate.GetComponentInChildren<Camera>();

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

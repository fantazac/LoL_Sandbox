using UnityEngine;
using System.Collections;

public abstract class CharacterBase : MonoBehaviour
{
    private Character _character;
    public Character Character
    {
        get
        {
            InitializeCharacter();
            return _character;
        }
    }

    private CharacterAbilityManager _characterAbilityManager;
    public CharacterAbilityManager CharacterAbilityManager
    {
        get
        {
            InitializeCharacterAbilityManager();
            return _characterAbilityManager;
        }
    }

    private CharacterInput _characterInput;
    public CharacterInput CharacterInput
    {
        get
        {
            InitializeCharacterInput();
            return _characterInput;
        }
    }

    private CharacterMouseManager _characterMouseManager;
    public CharacterMouseManager CharacterMouseManager
    {
        get
        {
            InitializeCharacterMouseManager();
            return _characterMouseManager;
        }
    }

    private CharacterMovement _characterMovement;
    public CharacterMovement CharacterMovement
    {
        get
        {
            InitializeCharacterMovement();
            return _characterMovement;
        }
    }

    private CharacterOrientation _characterOrientation;
    public CharacterOrientation CharacterOrientation
    {
        get
        {
            InitializeCharacterOrientation();
            return _characterOrientation;
        }
    }

    private CharacterStatsController _characterStatsController;
    public CharacterStatsController CharacterStatsController
    {
        get
        {
            InitializeCharacterStatsController();
            return _characterStatsController;
        }
    }

    private PhotonView _photonView;
    public PhotonView PhotonView
    {
        get
        {
            InitializePhotonView();
            return _photonView;
        }
    }

    protected virtual void Start()
    {
        InitializeCharacter();
        InitializeCharacterAbilityManager();
        InitializeCharacterInput();
        InitializeCharacterMouseManager();
        InitializeCharacterMovement();
        InitializeCharacterOrientation();
        InitializeCharacterStatsController();

        InitializePhotonView();
    }

    private void InitializeCharacter()
    {
        if (_character == null)
        {
            _character = GetComponent<Character>();
        }
    }

    private void InitializeCharacterAbilityManager()
    {
        if (_characterAbilityManager == null)
        {
            _characterAbilityManager = GetComponent<CharacterAbilityManager>();
        }
    }

    private void InitializeCharacterInput()
    {
        if (_characterInput == null)
        {
            _characterInput = GetComponent<CharacterInput>();
        }
    }

    private void InitializeCharacterMouseManager()
    {
        if (_characterMouseManager == null)
        {
            _characterMouseManager = GetComponent<CharacterMouseManager>();
        }
    }

    private void InitializeCharacterMovement()
    {
        if (_characterMovement == null)
        {
            _characterMovement = GetComponent<CharacterMovement>();
        }
    }

    private void InitializeCharacterOrientation()
    {
        if (_characterOrientation == null)
        {
            _characterOrientation = GetComponent<CharacterOrientation>();
        }
    }

    private void InitializeCharacterStatsController()
    {
        if (_characterStatsController == null)
        {
            _characterStatsController = GetComponent<CharacterStatsController>();
        }
    }

    private void InitializePhotonView()
    {
        if (_photonView == null)
        {
            _photonView = GetComponent<PhotonView>();
        }
    }

    public virtual void SerializeState(PhotonStream stream, PhotonMessageInfo info) { }//?
}

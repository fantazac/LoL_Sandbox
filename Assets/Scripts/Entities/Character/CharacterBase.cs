using UnityEngine;
using System.Collections;

public abstract class CharacterBase : MonoBehaviour {

    private Character _character;
    public Character Character
    {
        get
        {
            InitializeCharacter();
            return _character;
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
        InitializeCharacterInput();
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

    private void InitializeCharacterInput()
    {
        if (_characterInput == null)
        {
            _characterInput = GetComponent<CharacterInput>();
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
        if(_photonView == null)
        {
            _photonView = GetComponent<PhotonView>();
        }
    }

    public virtual void SerializeState(PhotonStream stream, PhotonMessageInfo info) { }//?
}

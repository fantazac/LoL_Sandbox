using UnityEngine;

public class CharacterInput : CharacterBase
{
    public delegate void OnPressedSHandler();
    public event OnPressedSHandler OnPressedS;

    public delegate void OnPressedYHandler();
    public event OnPressedYHandler OnPressedY;

    public delegate void OnPressedSpaceHandler();
    public event OnPressedSpaceHandler OnPressedSpace;

    public delegate void OnReleasedSpaceHandler();
    public event OnReleasedSpaceHandler OnReleasedSpace;

    public delegate void OnLeftClickHandler(Vector3 mousePosition);
    public event OnLeftClickHandler OnLeftClick;

    public delegate void OnRightClickHandler(Vector3 mousePosition);
    public event OnRightClickHandler OnRightClick;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        //TODO : On peut pas faire ca, car le CheckForOtherAbilitiesInputs utilise le shift, et donc ca checkrait aussi pour les touches sans le shift
        //exemple: Shift + L = TeleportMid, L = autre spell -> il ferait les 2 de la façon que c'est fait présentement
        CheckForCharacterAbilitiesInputs();
        CheckForSummonerSpellsInputs();
        CheckForOtherAbilitiesInputs();
        CheckForCameraControlInputs();
        CheckForMouseInputs();
    }

    private void CheckForCharacterAbilitiesInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.R);
        }
    }

    private void CheckForSummonerSpellsInputs()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.TELEPORT);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.HEAL);
        }
    }

    private void CheckForOtherAbilitiesInputs()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.SPAWN_ENEMY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.DESTROY_ALL_DUMMIES);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.SPAWN_ALLY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                CharacterAbilityManager.OnPressedInputForAbility(AbilityInput.TP_MID);
            }
        }
    }

    private void CheckForCameraControlInputs()
    {
        if (Input.GetKeyDown(KeyCode.S) && OnPressedS != null)
        {
            OnPressedS();
        }
        if (Input.GetKeyDown(KeyCode.Y) && OnPressedY != null)
        {
            OnPressedY();
        }
        if (Input.GetKeyDown(KeyCode.Space) && OnPressedSpace != null)
        {
            OnPressedSpace();
        }
        if (Input.GetKeyUp(KeyCode.Space) && OnPressedSpace != null)
        {
            OnReleasedSpace();
        }
    }

    private void CheckForMouseInputs()
    {
        if (Input.GetMouseButtonDown(0) && OnLeftClick != null)
        {
            OnLeftClick(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1) && OnRightClick != null)
        {
            OnRightClick(Input.mousePosition);
        }
    }
}

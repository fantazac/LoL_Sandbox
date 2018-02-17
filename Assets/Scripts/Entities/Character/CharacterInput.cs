using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    private CharacterAbilityManager characterAbilityManager;
    private CharacterLevelManager characterLevelManager;

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

    private void Start()
    {
        characterAbilityManager = StaticObjects.Character.CharacterAbilityManager;
        characterLevelManager = StaticObjects.Character.CharacterLevelManager;
    }

    private void Update()
    {
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
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.Q);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.W);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.E);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.R);
        }
    }

    private void CheckForSummonerSpellsInputs()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.D);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityInput.F);
        }
    }

    private void CheckForOtherAbilitiesInputs()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            characterAbilityManager.ResetCooldowns();
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            characterLevelManager.LevelUp();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityInput.SPAWN_ENEMY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityInput.DESTROY_ALL_DUMMIES);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityInput.SPAWN_ALLY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityInput.TP_MID);
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

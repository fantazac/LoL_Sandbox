using UnityEngine;

public class InputManager : MonoBehaviour
{
    private AbilityManager characterAbilityManager;
    private LevelManager characterLevelManager;

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

    public delegate void OnAbilityLevelUpHandler(int abilityId);
    public event OnAbilityLevelUpHandler OnAbilityLevelUp;

    private void Start()
    {
        characterAbilityManager = StaticObjects.Character.AbilityManager;
        characterLevelManager = StaticObjects.Character.LevelManager;
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnAbilityLevelUp(0);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnAbilityLevelUp(1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnAbilityLevelUp(2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnAbilityLevelUp(3);
            }
            /*if (Input.GetKeyDown(KeyCode.Equals))
            {
                characterLevelManager.ReachMaxLevel();
            }*/
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.CharacterAbility, 0);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.CharacterAbility, 1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.CharacterAbility, 2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.CharacterAbility, 3);
            }
            /*if (Input.GetKeyDown(KeyCode.Equals))
            {
                characterLevelManager.PrepareLevelUp();
            }*/
        }
    }

    private void CheckForSummonerSpellsInputs()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityCategory.SummonerAbility, 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            characterAbilityManager.OnPressedInputForAbility(AbilityCategory.SummonerAbility, 1);
        }
    }

    private void CheckForOtherAbilitiesInputs()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            characterAbilityManager.ResetCooldowns();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.OfflineAbility, 2);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.OfflineAbility, 0);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.OfflineAbility, 1);
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                characterLevelManager.ReachMaxLevel();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                characterAbilityManager.OnPressedInputForAbility(AbilityCategory.OtherCharacterAbility, 0);
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                characterLevelManager.PrepareLevelUp();
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

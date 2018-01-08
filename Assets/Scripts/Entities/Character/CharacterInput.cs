using UnityEngine;
using System.Collections;
using System;

public class CharacterInput : CharacterBase
{
    public delegate void OnPressedCharacterAbilityHandler(int abilityId);
    public event OnPressedCharacterAbilityHandler OnPressedCharacterAbility;

    public delegate void OnPressedOtherAbilityHandler(int abilityId);
    public event OnPressedOtherAbilityHandler OnPressedOtherAbility;

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.SPAWN_ENEMY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.M) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.DESTROY_ALL_DUMMIES);
            }
            if (Input.GetKeyDown(KeyCode.N) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.SPAWN_ALLY_DUMMY);
            }
            if (Input.GetKeyDown(KeyCode.L) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.TP_MID);
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && OnPressedS != null)
        {
            OnPressedS();
        }
        if (Input.GetKeyDown(KeyCode.D) && OnPressedOtherAbility != null)
        {
            OnPressedOtherAbility((int)OtherAbilities.TELEPORT);
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

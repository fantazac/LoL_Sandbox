﻿using UnityEngine;

public class CharacterInput : CharacterBase
{
    public delegate void OnPressedCharacterAbilityHandler(int abilityId, Vector3 mousePosition);
    public event OnPressedCharacterAbilityHandler OnPressedCharacterAbility;

    public delegate void OnPressedOtherAbilityHandler(int abilityId, Vector3 mousePosition);
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
        //OtherAbilities
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.B) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.SPAWN_ENEMY_DUMMY, Input.mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.M) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.DESTROY_ALL_DUMMIES, Input.mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.N) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.SPAWN_ALLY_DUMMY, Input.mousePosition);
            }
            if (Input.GetKeyDown(KeyCode.L) && OnPressedOtherAbility != null)
            {
                OnPressedOtherAbility((int)OtherAbilities.TP_MID, Input.mousePosition);
            }
        }
        //CharacterAbilities
        if (Input.GetKeyDown(KeyCode.Q) && OnPressedCharacterAbility != null)
        {
            OnPressedCharacterAbility((int)CharacterAbilities.Q, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.W) && OnPressedCharacterAbility != null)
        {
            OnPressedCharacterAbility((int)CharacterAbilities.W, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.E) && OnPressedCharacterAbility != null)
        {
            OnPressedCharacterAbility((int)CharacterAbilities.E, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.R) && OnPressedCharacterAbility != null)
        {
            OnPressedCharacterAbility((int)CharacterAbilities.R, Input.mousePosition);
        }
        //SummonerAbilities
        if (Input.GetKeyDown(KeyCode.D) && OnPressedOtherAbility != null)
        {
            OnPressedOtherAbility((int)OtherAbilities.TELEPORT, Input.mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.F) && OnPressedOtherAbility != null)
        {
            OnPressedOtherAbility((int)OtherAbilities.HEAL, Input.mousePosition);
        }
        //CameraControl
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
        //Mouse
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;
    protected float castTime;
    protected WaitForSeconds delayCastTime;
    protected RaycastHit hit;

    public int AbilityId { get; set; }
    public bool CanCastOtherAbilitiesWithCasting { get; protected set; }
    public bool CanMoveWhileCasting { get; protected set; }
    public bool CanStopMovement { get; protected set; }
    public bool OfflineOnly { get; protected set; }
    public bool HasCastTime { get; protected set; }

    public delegate void OnAbilityUsedWithOtherAbilityCastsUnallowedHandler();
    public event OnAbilityUsedWithOtherAbilityCastsUnallowedHandler OnAbilityUsedWithOtherAbilityCastsUnallowed;

    public delegate void OnAbilityUsedWithOtherAbilityCastsUnallowedFinishedHandler();
    public event OnAbilityUsedWithOtherAbilityCastsUnallowedFinishedHandler OnAbilityUsedWithOtherAbilityCastsUnallowedFinished;

    public delegate void OnAbilityUsedWithMovementUnallowedDuringCastHandler();
    public event OnAbilityUsedWithMovementUnallowedDuringCastHandler OnAbilityUsedWithMovementUnallowedDuringCast;

    public delegate void OnAbilityUsedWithMovementUnallowedDuringCastFinishedHandler();
    public event OnAbilityUsedWithMovementUnallowedDuringCastFinishedHandler OnAbilityUsedWithMovementUnallowedDuringCastFinished;

    public delegate void SendToServer_AbilityHandler(int abilityId, Vector3 destination);
    public event SendToServer_AbilityHandler SendToServer_Ability;

    protected Ability()
    {
        AbilityId = -1;
    }

    protected virtual void Start()
    {
        character = GetComponent<Character>();
        ModifyValues();
    }

    public abstract void OnPressedInput(Vector3 mousePosition);
    public abstract void UseAbility(Vector3 destination);

    protected virtual bool CanUseSkill(Vector3 mousePosition)
    {
        return !character.CharacterAbilityManager.isUsingAbilityWithOtherAbilityCastsUnallowed && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    protected virtual void ModifyValues() { }

    protected void StartAbilityCast()
    {
        if (OnAbilityUsedWithOtherAbilityCastsUnallowed != null && !CanCastOtherAbilitiesWithCasting)
        {
            OnAbilityUsedWithOtherAbilityCastsUnallowed();
        }
        if (OnAbilityUsedWithMovementUnallowedDuringCast != null && !CanMoveWhileCasting)
        {
            OnAbilityUsedWithMovementUnallowedDuringCast();
        }
    }

    protected void FinishAbilityCast()
    {
        if (OnAbilityUsedWithOtherAbilityCastsUnallowedFinished != null && !CanCastOtherAbilitiesWithCasting)
        {
            OnAbilityUsedWithOtherAbilityCastsUnallowedFinished();
        }
        if (OnAbilityUsedWithMovementUnallowedDuringCastFinished != null && !CanMoveWhileCasting)
        {
            OnAbilityUsedWithMovementUnallowedDuringCastFinished();
        }
    }

    protected void SendToServer(Vector3 destination)
    {
        if (SendToServer_Ability != null)
        {
            SendToServer_Ability(AbilityId, destination);
        }
    }

    protected virtual IEnumerator AbilityWithoutCastTime()
    {
        yield return null;
    }

    protected virtual IEnumerator AbilityWithCastTime()
    {
        yield return null;
    }
}

public interface CharacterAbility { }
public interface OtherAbility { }

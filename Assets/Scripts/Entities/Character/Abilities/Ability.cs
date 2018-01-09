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
    public bool CanStopMovement { get; protected set; }
    public bool OfflineOnly { get; protected set; }
    public bool HasCastTime { get; protected set; }

    public delegate void OnAbilityCastHandler();
    public event OnAbilityCastHandler OnAbilityCast;

    public delegate void OnAbilityCastFinishedHandler();
    public event OnAbilityCastFinishedHandler OnAbilityCastFinished;

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
        return !character.CharacterAbilityManager.isCastingAbility && MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit);
    }

    protected virtual void ModifyValues() { }

    protected void StartAbilityCast()
    {
        if (OnAbilityCast != null)
        {
            OnAbilityCast();
        }
    }

    protected void FinishAbilityCast()
    {
        if (OnAbilityCastFinished != null)
        {
            OnAbilityCastFinished();
        }
    }

    protected void SendToServer(Vector3 destination)
    {
        if(SendToServer_Ability != null)
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

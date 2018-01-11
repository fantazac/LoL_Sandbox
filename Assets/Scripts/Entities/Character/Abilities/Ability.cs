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

    public delegate void OnAbilityUsedHandler(Ability ability);
    public event OnAbilityUsedHandler OnAbilityUsed;

    public delegate void OnAbilityFinishedHandler(Ability ability);
    public event OnAbilityFinishedHandler OnAbilityFinished;

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

    public abstract bool CanBeCast(Vector3 mousePosition);
    public abstract Vector3 GetDestination();
    public abstract void UseAbility(Vector3 destination);

    protected virtual void ModifyValues() { }

    protected void StartAbilityCast()
    {
        if (OnAbilityUsed != null)
        {
            OnAbilityUsed(this);
        }
    }

    protected void FinishAbilityCast()
    {
        if (OnAbilityFinished != null)
        {
            OnAbilityFinished(this);
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

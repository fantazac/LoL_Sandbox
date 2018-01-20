using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    protected float castTime;
    protected float damage;
    protected WaitForSeconds delayCastTime;
    protected RaycastHit hit;
    protected float range;
    protected float speed;

    public bool CanCastOtherAbilitiesWithCasting { get; protected set; }
    public bool CanMoveWhileCasting { get; protected set; }
    public bool CanRotateWhileCasting { get; protected set; }
    public bool CanStopMovement { get; protected set; }
    public bool OfflineOnly { get; protected set; }
    public bool HasCastTime { get; protected set; }

    public delegate void OnAbilityUsedHandler(Ability ability);
    public event OnAbilityUsedHandler OnAbilityUsed;

    public delegate void OnAbilityFinishedHandler(Ability ability);
    public event OnAbilityFinishedHandler OnAbilityFinished;

    protected virtual void Start()
    {
        character = GetComponent<Character>();
        ModifyValues();
    }

    //public abstract bool CanBeCast(Entity target, CharacterAbilityManager characterAbilityManager);
    public abstract bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager);
    public abstract Vector3 GetDestination();
    //public abstract void UseAbility(Entity target);
    public abstract void UseAbility(Vector3 destination);

    protected virtual void ModifyValues()
    {
        range /= StaticObjects.DivisionFactor;
        speed /= StaticObjects.DivisionFactor;
    }

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

    protected virtual IEnumerator AbilityWithCastTime() { yield return null; }
    protected virtual IEnumerator AbilityWithoutCastTime() { yield return null; }
}

public interface CharacterAbility { }
public interface OtherAbility { }

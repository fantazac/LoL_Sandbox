using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Character character;

    protected AbilityAffectedUnitType affectedUnitType;
    protected AbilityEffectType effectType;
    protected DamageType damageType;

    protected float castTime;
    protected float damage;
    protected WaitForSeconds delayCastTime;
    protected float durationOfActive;
    protected RaycastHit hit;
    protected Vector3 positionOnCast;
    protected float range;
    protected float speed;

    public bool CanBeCancelled { get; protected set; }
    public bool CanCastOtherAbilitiesWhileActive { get; private set; }
    public bool CanMoveWhileCasting { get; protected set; }
    public bool CanRotateWhileCasting { get; protected set; }
    public bool OfflineOnly { get; protected set; }
    public bool HasCastTime { get; protected set; }

    public List<Ability> CastableAbilitiesWhileActive { get; protected set; }

    public delegate void OnAbilityUsedHandler(Ability ability);
    public event OnAbilityUsedHandler OnAbilityUsed;

    public delegate void OnAbilityFinishedHandler(Ability ability);
    public event OnAbilityFinishedHandler OnAbilityFinished;

    protected Ability()
    {
        CastableAbilitiesWhileActive = new List<Ability>();
    }

    protected virtual void Start()
    {
        character = GetComponent<Character>();
        CanCastOtherAbilitiesWhileActive = CastableAbilitiesWhileActive.Count > 0;

        ModifyValues();
    }

    public abstract bool CanBeCast(Entity target);
    public abstract bool CanBeCast(Vector3 mousePosition);
    public abstract Vector3 GetDestination();
    public abstract void UseAbility(Entity target);
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

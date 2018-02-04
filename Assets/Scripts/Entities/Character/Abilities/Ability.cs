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

    public int ID { get; set; }

    protected float castTime;
    protected float cooldown;
    protected float cooldownRemaining;
    protected float damage;
    protected WaitForSeconds delayCastTime;
    protected float durationOfActive;
    protected RaycastHit hit;
    protected Vector3 destinationOnCast;
    protected Vector3 positionOnCast;
    protected Quaternion rotationOnCast;
    protected float range;
    protected float speed;
    protected bool startCooldownOnAbilityCast;

    public Sprite abilitySprite;
    protected string abilitySpritePath;

    public bool CanBeCastAtAnytime { get; protected set; }
    public bool CanBeCancelled { get; protected set; }
    public bool CanCastOtherAbilitiesWhileActive { get; private set; }
    public bool CanMoveWhileCasting { get; protected set; }
    public bool CannotRotateWhileCasting { get; protected set; }
    public bool IsADash { get; protected set; }
    public bool IsOnCooldown { get; protected set; }
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
        SetAbilitySpritePath();
    }

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        if (!StaticObjects.OnlineMode || character.PhotonView.isMine)
        {
            abilitySprite = Resources.Load<Sprite>(abilitySpritePath);
        }
    }

    protected virtual void Start()
    {
        CanCastOtherAbilitiesWhileActive = CastableAbilitiesWhileActive.Count > 0;

        ModifyValues();
    }

    public abstract bool CanBeCast(Entity target);
    public abstract bool CanBeCast(Vector3 mousePosition);
    public abstract Vector3 GetDestination();
    public abstract void UseAbility(Entity target);
    public abstract void UseAbility(Vector3 destination);

    protected abstract void SetAbilitySpritePath();

    protected virtual void RotationOnAbilityCast(Vector3 destination)
    {
        character.CharacterOrientation.RotateCharacterTowardsCastPoint(destination);
    }

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
        StartCooldown(startCooldownOnAbilityCast);
    }

    protected void FinishAbilityCast()
    {
        if (OnAbilityFinished != null)
        {
            OnAbilityFinished(this);
        }
        StartCooldown(!startCooldownOnAbilityCast);
    }

    protected void StartCooldown(bool calledInStartAbilityCast)
    {
        if (calledInStartAbilityCast == startCooldownOnAbilityCast && (!StaticObjects.OnlineMode || character.PhotonView.isMine))
        {
            StartCoroutine(PutAbilityOffCooldown());
        }
    }

    protected void SetPositionAndRotationOnCast(Vector3 position)
    {
        positionOnCast = position;
        rotationOnCast = Quaternion.LookRotation((destinationOnCast - transform.position).normalized);
    }

    protected virtual IEnumerator PutAbilityOffCooldown()
    {
        IsOnCooldown = true;
        cooldownRemaining = cooldown;

        yield return null;

        character.AbilityUIManager.SetAbilityOnCooldown(ID);

        while (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;

            character.AbilityUIManager.UpdateAbilityCooldown(ID, cooldown, cooldownRemaining);

            yield return null;
        }

        character.AbilityUIManager.SetAbilityOffCooldown(ID);
        IsOnCooldown = false;
    }

    public void ResetCooldown()
    {
        cooldownRemaining = 0;
    }

    public void ReduceCooldown(float cooldownReductionAmount)
    {
        cooldownRemaining -= cooldownReductionAmount;
    }

    protected virtual IEnumerator AbilityWithCastTime() { yield return null; }
    protected virtual IEnumerator AbilityWithoutCastTime() { yield return null; }
}

public interface PassiveCharacterAbility { }
public interface CharacterAbility { }
public interface SummonerAbility { }
public interface OtherAbility { }


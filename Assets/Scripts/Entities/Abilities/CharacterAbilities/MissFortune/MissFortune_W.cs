using System.Collections;
using UnityEngine;

public class MissFortune_W : SelfTargeted
{
    private float baseCooldownReductionOnPassiveHit;
    private float cooldownReductionOnPassiveHit;

    private IEnumerator passiveBuffCooldownAfterTakingDamage;
    private float timeBeforeEnablingPassive;
    private WaitForSeconds delayPassiveBuff;

    protected MissFortune_W()
    {
        abilityName = "Strut";

        abilityType = AbilityType.PASSIVE;

        MaxLevel = 5;

        resourceCost = 30;
        baseCooldown = 12;

        affectedByCooldownReduction = true;

        baseCooldownReductionOnPassiveHit = 2;

        timeBeforeEnablingPassive = 5;
        delayPassiveBuff = new WaitForSeconds(timeBeforeEnablingPassive);
    }

    protected override void SetResourcePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/MissFortune/MissFortuneW";
    }

    protected override void Start()
    {
        base.Start();

        AbilityBuffs = new AbilityBuff[] { gameObject.AddComponent<MissFortune_W_PassiveBuff>(), gameObject.AddComponent<MissFortune_W_Buff>() };
    }

    protected override void SetCooldownForAbilityAffectedByCooldownReduction(float cooldownReduction)
    {
        base.SetCooldownForAbilityAffectedByCooldownReduction(cooldownReduction);

        cooldownReductionOnPassiveHit = baseCooldownReductionOnPassiveHit * (1 - (cooldownReduction * 0.01f));
    }

    public override void EnableAbilityPassive()
    {
        GetComponent<MissFortune_P>().OnPassiveHit += OnPassiveHit;

        character.StatsManager.Health.OnResourceReduced += OnDamageTaken;
        //TODO: something.OnRevive += OnRevive;
        AddNewDebuffToEntityHit(character);
    }

    private void OnPassiveHit()
    {
        if (IsOnCooldown)
        {
            cooldownRemaining -= cooldownReductionOnPassiveHit;
        }
    }

    private void OnDamageTaken()
    {
        AbilityBuffs[0].ConsumeBuff(character);

        if (passiveBuffCooldownAfterTakingDamage != null)
        {
            StopCoroutine(passiveBuffCooldownAfterTakingDamage);
        }
        passiveBuffCooldownAfterTakingDamage = PassiveBuffCooldownAfterTakingDamage();
        StartCoroutine(passiveBuffCooldownAfterTakingDamage);
    }

    private void OnRevive()
    {
        if (passiveBuffCooldownAfterTakingDamage != null)
        {
            StopCoroutine(passiveBuffCooldownAfterTakingDamage);
            passiveBuffCooldownAfterTakingDamage = null;
        }
        AddNewDebuffToEntityHit(character);
    }

    public override void UseAbility(Vector3 destination)
    {
        StartAbilityCast();

        AbilityBuffs[1].AddNewBuffToAffectedEntity(character);

        FinishAbilityCast();
    }

    private IEnumerator PassiveBuffCooldownAfterTakingDamage()
    {
        yield return delayPassiveBuff;

        passiveBuffCooldownAfterTakingDamage = null;
        AddNewDebuffToEntityHit(character);
    }

    private void AddNewDebuffToEntityHit(Entity entityHit)
    {
        AbilityBuffs[0].AddNewBuffToAffectedEntity(entityHit);
    }
}

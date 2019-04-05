using System.Collections;
using UnityEngine;

public class MissFortune_W : SelfTargeted
{
    private readonly float baseCooldownReductionOnPassiveHit;
    private float cooldownReductionOnPassiveHit;

    private IEnumerator passiveBuffCooldownAfterTakingDamage;
    private readonly WaitForSeconds delayPassiveBuff;

    protected MissFortune_W()
    {
        abilityName = "Strut";

        abilityType = AbilityType.PASSIVE;

        MaxLevel = 5;

        resourceCost = 30;
        baseCooldown = 12;

        affectedByCooldownReduction = true;

        baseCooldownReductionOnPassiveHit = 2;

        delayPassiveBuff = new WaitForSeconds(5);
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

        champion.StatsManager.Health.OnResourceReduced += OnDamageTaken;
        //TODO: something.OnRevive += OnRevive;
        AddNewDebuffToAffectedUnit(champion);
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
        AbilityBuffs[0].ConsumeBuff(champion);

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

        AddNewDebuffToAffectedUnit(champion);
    }

    public override void UseAbility()
    {
        StartAbilityCast();

        AbilityBuffs[1].AddNewBuffToAffectedUnit(champion);

        FinishAbilityCast();
    }

    private IEnumerator PassiveBuffCooldownAfterTakingDamage()
    {
        yield return delayPassiveBuff;

        passiveBuffCooldownAfterTakingDamage = null;
        AddNewDebuffToAffectedUnit(champion);
    }

    private void AddNewDebuffToAffectedUnit(Unit affectedUnit)
    {
        AbilityBuffs[0].AddNewBuffToAffectedUnit(affectedUnit);
    }
}

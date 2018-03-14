using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : UnitTargeted, CharacterAbility
{
    [SerializeField]
    protected GameObject areaOfEffectPrefab;

    protected float durationAoE;

    protected Lucian_Q()
    {
        abilityName = "Piercing Light";

        abilityType = AbilityType.AreaOfEffect;
        affectedUnitType = AbilityAffectedUnitType.ENEMIES;
        effectType = AbilityEffectType.AREA_OF_EFFECT;
        damageType = DamageType.PHYSICAL;

        range = 500;
        damage = 85;// 85/120/155/190/225 + BONUS AD % 60/70/80/90/100
        resourceCost = 50;// 50/60/70/80/90
        cooldown = 9;// 9/8/7/6/5
        castTime = 0.4f;
        delayCastTime = new WaitForSeconds(castTime);

        durationAoE = 0.15f;
    }

    protected override void SetSpritePaths()
    {
        abilitySpritePath = "Sprites/Characters/CharacterAbilities/Lucian/LucianQ";
    }

    protected override void Start()
    {
        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_W>());
        CastableAbilitiesWhileActive.Add(GetComponent<Lucian_R>());

        base.Start();

        character.CharacterLevelManager.OnLevelUp += OnLevelUp;
    }

    public override void OnLevelUp(int level)
    {
        castTime = (0.409f - (0.009f * level));
        delayCastTime = new WaitForSeconds(castTime);
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        Quaternion currentRotation = transform.rotation;
        transform.LookAt(destinationOnCast);
        SetPositionAndRotationOnCast(transform.position + (transform.forward * areaOfEffectPrefab.transform.localScale.z * 0.5f));
        transform.rotation = currentRotation;

        yield return delayCastTime;

        character.CharacterOrientation.RotateCharacterInstantly(destinationOnCast);

        AreaOfEffect aoe = ((GameObject)Instantiate(areaOfEffectPrefab, positionOnCast, rotationOnCast)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(new List<Entity>(), character.Team, affectedUnitType, durationAoE);
        aoe.OnAbilityEffectHit += OnAreaOfEffectHit;
        FinishAbilityCast();
    }
}

using UnityEngine;

public class Tristana_R_Debuff : AbilityBuff
{
    private float knockbackSpeed;

    protected Tristana_R_Debuff()
    {
        buffName = "Buster Shot";

        isADebuff = true;

        buffFlatValue = 6;// 600/800/1000 * StaticObjects.MultiplyingFactor
        buffFlatValuePerLevel = 2;

        knockbackSpeed = 20;

        buffCrowdControlEffect = CrowdControlEffect.KNOCKBACK;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Tristana/TristanaR_Debuff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedUnit.DisplacementManager.SetupDisplacement(normalizedVector * buff.BuffValue, knockbackSpeed, this);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, buffFlatValue);
    }
}
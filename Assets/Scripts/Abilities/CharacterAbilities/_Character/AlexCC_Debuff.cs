using UnityEngine;

public class AlexCC_Debuff : AbilityBuff
{
    private float knockUpSpeed;
    private Vector3 knockUpDestination;

    protected AlexCC_Debuff()
    {
        buffName = "AlexCC_Debuff";

        isADebuff = true;

        buffDuration = 2;

        knockUpSpeed = 3;
        knockUpDestination = Vector3.up * knockUpSpeed * buffDuration * 0.5f;

        buffStatusEffect = StatusEffect.SUPPRESSION;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/_Character/AlexCC_Debuff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddStatusEffect(buffStatusEffect);
        //affectedUnit.ForcedActionManager.SetupForcedAction(buffStatusEffect, this, character);
        //affectedUnit.DisplacementManager.SetupDisplacement(knockUpDestination, knockUpSpeed, this, true);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.NORMAL, this, 200);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.MAGIC, this, 700);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.PHYSICAL, this, 1000);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveStatusEffect(buffStatusEffect);
        //affectedUnit.ForcedActionManager.StopCurrentForcedAction(this);
        //affectedUnit.ShieldManager.RemoveShield(ShieldType.NORMAL, this);
        //affectedUnit.ShieldManager.RemoveShield(ShieldType.MAGIC, this);
        //affectedUnit.ShieldManager.RemoveShield(ShieldType.PHYSICAL, this);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, GetBuffDuration(affectedUnit));
    }
}

using UnityEngine;

public class CC_W_Debuff : AbilityBuff
{
    //private float knockupSpeed;
    //private Vector3 knockupDestination;

    protected CC_W_Debuff()
    {
        buffName = "CC BTW HAHA";

        isADebuff = true;

        buffDuration = 2;

        //knockupSpeed = 3;
        //knockupDestination = Vector3.up * knockupSpeed * buffDuration * 0.5f;

        buffCrowdControlEffect = CrowdControlEffect.STUN;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW_Debuff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        //affectedUnit.ForcedActionManager.SetupForcedAction(buffCrowdControlEffect, this, character);
        //affectedUnit.DisplacementManager.SetupDisplacement(knockupDestination, knockupSpeed, this, true);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.NORMAL, this, 200);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.MAGIC, this, 700);
        //affectedUnit.ShieldManager.AddNewShield(ShieldType.PHYSICAL, this, 1000);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
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
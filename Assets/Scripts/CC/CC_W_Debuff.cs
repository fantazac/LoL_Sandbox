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

        buffCrowdControlEffect = CrowdControlEffect.TAUNT;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/CC/CCW_Debuff";
    }

    protected override void ApplyBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.AddCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityForcedActionManager.SetupForcedAction(buffCrowdControlEffect, this, character);
        //affectedEntity.EntityDisplacementManager.SetupDisplacement(knockupDestination, knockupSpeed, this, true);
        affectedEntity.EntityShieldManager.AddNewShield(ShieldType.NORMAL, this, 200);
        //affectedEntity.EntityShieldManager.AddNewShield(ShieldType.MAGIC, this, 700);
        //affectedEntity.EntityShieldManager.AddNewShield(ShieldType.PHYSICAL, this, 1000);
    }

    protected override void RemoveBuffEffect(Entity affectedEntity, float buffValue, int currentStacks)
    {
        affectedEntity.EntityStatusManager.RemoveCrowdControlEffect(buffCrowdControlEffect);
        affectedEntity.EntityForcedActionManager.StopCurrentForcedAction(this);
        affectedEntity.EntityShieldManager.RemoveShield(ShieldType.NORMAL, this);
        //affectedEntity.EntityShieldManager.RemoveShield(ShieldType.MAGIC, this);
        //affectedEntity.EntityShieldManager.RemoveShield(ShieldType.PHYSICAL, this);
    }

    protected override Buff CreateNewBuff(Entity affectedEntity)
    {
        return new Buff(this, affectedEntity, 0, GetBuffDuration(affectedEntity));
    }
}
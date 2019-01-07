public class Varus_R_Debuff : AbilityBuff
{
    protected Varus_R_Debuff()
    {
        buffName = "Chain of Corruption";

        isADebuff = true;

        buffDuration = 2;

        buffStatusEffect = StatusEffect.ROOT;
    }

    protected override void SetSpritePaths()
    {
        buffSpritePath = "Sprites/Characters/CharacterAbilities/Varus/VarusR_Debuff";
    }

    protected override void ApplyBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.AddStatusEffect(buffStatusEffect);
    }

    protected override void RemoveBuffEffect(Unit affectedUnit, Buff buff)
    {
        affectedUnit.StatusManager.RemoveStatusEffect(buffStatusEffect);
    }

    protected override Buff CreateNewBuff(Unit affectedUnit)
    {
        return new Buff(this, affectedUnit, 0, GetBuffDuration(affectedUnit));
    }
}

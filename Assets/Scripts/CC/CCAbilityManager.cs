public class CCAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<CC_Q>(), gameObject.AddComponent<Lucian_E>(), gameObject.AddComponent<MissFortune_E>(), gameObject.AddComponent<Lucian_R>() };
        PassiveCharacterAbilities = new Ability[] { };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Teleport>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

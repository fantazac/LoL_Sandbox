public class CCAbilityManager : AbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[]
        {
            gameObject.AddComponent<CC_Q>(), 
            gameObject.AddComponent<CC_W>(), 
            gameObject.AddComponent<MissFortune_E>(), 
            gameObject.AddComponent<Lucian_R>()
        };
        PassiveCharacterAbilities = new Ability[] { };
        SummonerAbilities = new Ability[]
        {
            gameObject.AddComponent<Heal>(), 
            gameObject.AddComponent<Flash>()
        };

        base.InitAbilities();
    }
}

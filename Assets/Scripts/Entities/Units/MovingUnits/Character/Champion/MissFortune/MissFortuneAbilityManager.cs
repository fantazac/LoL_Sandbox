public class MissFortuneAbilityManager : AbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[]
        {
            gameObject.AddComponent<MissFortune_Q>(),
            gameObject.AddComponent<MissFortune_W>(), 
            gameObject.AddComponent<MissFortune_E>(),
            gameObject.AddComponent<MissFortune_R>()
        };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<MissFortune_P>() };
        SummonerAbilities = new Ability[]
        {
            gameObject.AddComponent<Heal>(), 
            gameObject.AddComponent<Flash>()
        };

        base.InitAbilities();
    }
}

public class LucianAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Lucian_Q>(), gameObject.AddComponent<Lucian_W>(), gameObject.AddComponent<Lucian_E>(), gameObject.AddComponent<Lucian_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<Lucian_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

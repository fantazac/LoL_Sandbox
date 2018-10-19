public class TristanaAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Tristana_Q>(), gameObject.AddComponent<Tristana_W>(), gameObject.AddComponent<Tristana_E>(), gameObject.AddComponent<Tristana_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<Tristana_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

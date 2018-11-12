public class VarusAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Tristana_Q>(), gameObject.AddComponent<Lucian_W>(), gameObject.AddComponent<Varus_E>(), gameObject.AddComponent<Varus_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<Tristana_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

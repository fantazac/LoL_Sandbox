public class VarusAbilityManager : CharacterAbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Ezreal_Q>(), gameObject.AddComponent<Varus_W>(), gameObject.AddComponent<Varus_E>(), gameObject.AddComponent<Varus_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<MissFortune_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Heal>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

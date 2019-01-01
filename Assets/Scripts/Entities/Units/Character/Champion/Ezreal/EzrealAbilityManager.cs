public class EzrealAbilityManager : AbilityManager
{
    protected override void InitAbilities()
    {
        CharacterAbilities = new Ability[] { gameObject.AddComponent<Ezreal_Q>(), gameObject.AddComponent<Ezreal_W>(), gameObject.AddComponent<Ezreal_E>(), gameObject.AddComponent<Ezreal_R>() };
        PassiveCharacterAbilities = new Ability[] { gameObject.AddComponent<Ezreal_P>() };
        SummonerAbilities = new Ability[] { gameObject.AddComponent<Teleport>(), gameObject.AddComponent<Flash>() };

        base.InitAbilities();
    }
}

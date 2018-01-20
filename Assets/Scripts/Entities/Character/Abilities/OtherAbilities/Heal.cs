using UnityEngine;

public class Heal : Ability, OtherAbility
{
    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return true;
    }

    public override Vector3 GetDestination()
    {
        return character.transform.position;
    }

    public override void UseAbility(Vector3 destination)
    {
        GetComponent<CharacterStats>().Health.Heal(150);
    }
}

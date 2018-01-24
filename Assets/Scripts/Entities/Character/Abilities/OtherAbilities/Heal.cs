using UnityEngine;

public class Heal : SelfTargeted, OtherAbility
{
    public override void UseAbility(Vector3 destination)
    {
        GetComponent<CharacterStats>().Health.Restore(150);
    }
}

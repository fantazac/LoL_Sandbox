using UnityEngine;

public class Heal : Ability, OtherAbility
{
    public override void OnPressedInput(Vector3 mousePosition)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer(mousePosition);
        }
        else
        {
            UseAbility(mousePosition);
        }
    }

    public override void UseAbility(Vector3 destination)
    {
        GetComponent<CharacterStats>().Health.Heal(150);
    }
}

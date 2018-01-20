using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : Ability, OtherAbility
{
    protected TeleportToMid()
    {
        CanStopMovement = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return !StaticObjects.OnlineMode || !OfflineOnly;
    }

    public override Vector3 GetDestination()
    {
        return character.CharacterMovement.CharacterHeightOffset;
    }

    public override void UseAbility(Vector3 destination)
    {
        character.CharacterMovement.StopAllMovement(this);
        character.transform.position = character.CharacterMovement.CharacterHeightOffset;
        character.CharacterMovement.NotifyCharacterMoved();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : AutoTargetedBlink, OtherAbility
{
    protected TeleportToMid()
    {
        CanStopMovement = true;
    }

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return (!StaticObjects.OnlineMode || !OfflineOnly);
    }

    public override Vector3 GetDestination()
    {
        return character.CharacterMovement.CharacterHeightOffset;
    }
}

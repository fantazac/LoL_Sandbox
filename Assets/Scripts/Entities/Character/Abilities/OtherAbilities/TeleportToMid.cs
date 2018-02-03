using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToMid : AutoTargetedBlink, OtherAbility
{
    public override bool CanBeCast(Vector3 mousePosition)
    {
        return (!StaticObjects.OnlineMode || !OfflineOnly);
    }

    public override Vector3 GetDestination()
    {
        return character.CharacterMovement.CharacterHeightOffset;
    }

    protected override void SetAbilitySpritePath() { }
}

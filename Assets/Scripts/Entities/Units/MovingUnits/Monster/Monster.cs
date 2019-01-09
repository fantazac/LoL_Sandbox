public class Monster : MovingUnit
{
    protected override void SetPortraitSpritePath()
    {
        portraitSpritePath = "Sprites/Portraits/Character/Dummy/Dummy";//TODO
    }

    protected override MovementManager GetMovementManager()
    {
        return null;
    }
}

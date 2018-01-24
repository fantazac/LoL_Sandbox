public class MovementSpeed : Stat
{
    private const float MOVEMENT_SPEED_MODIFIER = 0.01f;    // This is because our map is 100 times smaller than the real one.


    public float GetCurrentMovementSpeedForMovement()
    {
        return total * MOVEMENT_SPEED_MODIFIER;
    }

    public override string GetUIText()
    {
        return "MOVEMENT SPEED: " + GetTotal() + " ((" + GetBaseValue() + " + " + GetFlatBonus() +
               ") * " + GetPercentBonus() + "% * -" + GetPercentMalus() + "% - " + GetFlatMalus() + ")";
    }
}

public class Health : Resource
{
    public Health(float initialBaseValue) : base(initialBaseValue) { }
    public Health(float initialBaseValue, float perLevelValue) : base(initialBaseValue, perLevelValue) { }

    public bool IsDead()//TODO: I keep Health class for this method only, but it shouldn't be the one checking if the target is alive or not. When it's transfered somewhere else, delete the class.
    {
        return currentValue <= 0;
    }
}

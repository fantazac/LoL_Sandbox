using UnityEngine;

public class EffectSourceManager : MonoBehaviour
{
    private Unit unit;

    public delegate void OnUnitHitByAbilityHandler(Unit unit, Ability ability);
    public event OnUnitHitByAbilityHandler OnUnitHitByAbility;

    public delegate void OnUnitHitByBasicAttackHandler(Unit unit, Unit sourceUnit);
    public event OnUnitHitByBasicAttackHandler OnUnitHitByBasicAttack;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public void UnitHitByAbility(Ability ability)
    {
        OnUnitHitByAbility?.Invoke(unit, ability);
    }

    public void UnitHitByBasicAttack(Unit sourceUnit)
    {
        OnUnitHitByBasicAttack?.Invoke(unit, sourceUnit);
    }
}

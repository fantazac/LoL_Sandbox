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
        if(OnUnitHitByAbility != null)
        {
            OnUnitHitByAbility(unit, ability);
        }
    }

    public void UnitHitByBasicAttack(Unit sourceUnit)
    {
        if (OnUnitHitByBasicAttack != null)
        {
            OnUnitHitByBasicAttack(unit, sourceUnit);
        }
    }
}

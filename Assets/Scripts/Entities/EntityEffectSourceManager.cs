using UnityEngine;

public class EntityEffectSourceManager : MonoBehaviour
{
    private Entity entity;

    public delegate void OnEntityHitByAbilityHandler(Entity entity, Ability ability);
    public event OnEntityHitByAbilityHandler OnEntityHitByAbility;

    public delegate void OnEntityHitByBasicAttackHandler(Entity entity, Entity sourceEntity);
    public event OnEntityHitByBasicAttackHandler OnEntityHitByBasicAttack;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void EntityHitByAbility(Ability ability)
    {
        if(OnEntityHitByAbility != null)
        {
            OnEntityHitByAbility(entity, ability);
        }
    }

    public void EntityHitByBasicAttack(Entity sourceEntity)
    {
        if (OnEntityHitByBasicAttack != null)
        {
            OnEntityHitByBasicAttack(entity, sourceEntity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucian_Q : SkillShot, CharacterAbility
{
    private GameObject target;

    private float durationAoE;

    protected Lucian_Q()
    {
        range = 500;
        damage = 130;
        castTime = 0.3f;
        delayCastTime = new WaitForSeconds(castTime);

        durationAoE = 0.15f;

        CanStopMovement = true;
        HasCastTime = true;
    }

    public override bool CanBeCast(Vector3 mousePosition, CharacterAbilityManager characterAbilityManager)
    {
        return !characterAbilityManager.IsUsingAbilityPreventingAbilityCasts() && character.CharacterMouseManager.HoveredObjectIsEnemy(character.team);
    }

    public override Vector3 GetDestination()
    {
        return Vector3.right * character.CharacterMouseManager.HoveredObject.GetComponent<Character>().characterId;
    }

    public override void UseAbility(Vector3 destination)
    {
        target = FindEnemyCharacter((int)destination.x);

        if (Vector3.Distance(target.transform.position, transform.position) <= range)
        {
            base.UseAbility(target.transform.position);
        }
        else
        {
            character.CharacterMovement.SetMoveTowardsTarget(target.transform, range);
            character.CharacterMovement.CharacterIsInRange += base.UseAbility;
        }  
    }

    private GameObject FindEnemyCharacter(int characterId)
    {
        GameObject enemyCharacter = null;
        foreach (Character character in FindObjectsOfType<Character>())
        {
            if (character.characterId == characterId)
            {
                enemyCharacter = character.gameObject;
                break;
            }
        }
        return enemyCharacter;
    }

    protected override IEnumerator AbilityWithCastTime()
    {
        yield return delayCastTime;

        AreaOfEffect aoe = ((GameObject)Instantiate(projectilePrefab, transform.position + (transform.forward * projectilePrefab.transform.localScale.z * 0.5f), transform.rotation)).GetComponent<AreaOfEffect>();
        aoe.ActivateAreaOfEffect(new List<Health>(), character.team, false, damage, durationAoE);

        FinishAbilityCast();
    }
}

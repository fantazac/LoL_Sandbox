using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusManager : MonoBehaviour
{
    private Character character;

    private List<CrowdControlEffects> crowdControlEffectsOnCharacter;

    public bool CanUseBasicAttacks { get; private set; }
    public bool CanUseCharacterAbilities { get; private set; }
    public bool CanUseMovement { get; private set; }
    public bool CanUseSummonerAbilities { get; private set; }
    public bool IsBlinded { get; private set; }

    private CharacterStatusManager()
    {
        crowdControlEffectsOnCharacter = new List<CrowdControlEffects>();
    }

    private void Start()
    {
        character = GetComponent<Character>();
    }

    public void AddCrowdControlEffectOnCharacter(CrowdControlEffects crowdControlEffect)
    {
        crowdControlEffectsOnCharacter.Add(crowdControlEffect);
    }

    public void RemoveCrowdControlEffectFromCharacter(CrowdControlEffects crowdControlEffect)
    {
        crowdControlEffectsOnCharacter.Remove(crowdControlEffect);
    }

    /* !(crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.BLIND) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.CHARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.CRIPPLE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.DISARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.DISRUPT) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.DROWSY) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ENTANGLE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.FEAR) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.GROUND) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKASIDE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKBACK) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKDOWN) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKUP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.NEARSIGHT) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PACIFY) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.POLYMORPH) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PULL) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ROOT) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SILENCE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SLEEP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SLOW) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STASIS) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STUN) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUPPRESION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUSPENSION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.TAUNT));
                */

    private void SetCanUseBasicAttacks()
    {
        CanUseBasicAttacks = !(crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.CHARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.DISARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ENTANGLE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.FEAR) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKASIDE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKBACK) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKUP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PACIFY) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.POLYMORPH) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PULL) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SLEEP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STASIS) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STUN) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUPPRESION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUSPENSION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.TAUNT));
    }

    private void SetCanUseCharacterAbilities()
    {
        CanUseCharacterAbilities = !(crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.CHARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.FEAR) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKASIDE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKBACK) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKUP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PACIFY) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.POLYMORPH) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PULL) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SILENCE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SLEEP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STASIS) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STUN) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUPPRESION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUSPENSION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.TAUNT));
    }

    private void SetCanUseMovement()
    {
        CanUseMovement = !(crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.CHARM) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ENTANGLE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.FEAR) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKASIDE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKBACK) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.KNOCKUP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PULL) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.ROOT) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SLEEP) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STASIS) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STUN) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUPPRESION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUSPENSION) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.TAUNT));
    }

    private void SetCanUseSummonerAbilities()
    {
        CanUseSummonerAbilities = !(crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.PACIFY) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.POLYMORPH) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SILENCE) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.STASIS) ||
                crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.SUPPRESION));
    }

    private void SetIsBlinded()
    {
        IsBlinded = crowdControlEffectsOnCharacter.Contains(CrowdControlEffects.BLIND);
    }
}

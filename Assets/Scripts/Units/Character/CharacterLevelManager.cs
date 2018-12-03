using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private Character character;

    public int AbilityPoints { get; private set; }
    public int Level { get; private set; }

    private static int MaxLevel = 18;

    private int pointsAvailableForQ;
    private int pointsAvailableForW;
    private int pointsAvailableForE;
    private int pointsAvailableForR;

    public delegate void OnLevelUpHandler(int level);
    public event OnLevelUpHandler OnLevelUp;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        if (character.AbilityLevelUpUIManager)
        {
            character.AbilityLevelUpUIManager.OnAbilityLevelUp += OnAbilityLevelUp;
            character.CharacterInputManager.OnAbilityLevelUp += OnAbilityLevelUp;
            PrepareLevelUp();
        }
    }

    public void PrepareLevelUp()
    {
        if (Level < MaxLevel)
        {
            if (StaticObjects.OnlineMode)
            {
                SendToServer_LevelUp();
            }
            else
            {
                LevelUp();
            }
        }
    }

    public void SetLevelFromLoad(int characterLevel)
    {
        for (int i = 0; i < characterLevel; i++)
        {
            LevelUp();
        }
    }

    private void SendToServer_LevelUp()
    {
        character.PhotonView.RPC("ReceiveFromServer_LevelUp", PhotonTargets.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_LevelUp()
    {
        LevelUp();
    }

    private void LevelUp()
    {
        ++Level;
        if (SetPointsAvaiableForAbilities() > AbilityPoints)
        {
            ++AbilityPoints;
        }
        if (character.AbilityLevelUpUIManager && AbilityPoints > 0)
        {
            character.AbilityLevelUpUIManager.gameObject.SetActive(true);
            character.AbilityLevelUpUIManager.SetAbilityPoints(AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
        }
        if (character.LevelUIManager)
        {
            character.LevelUIManager.SetLevel(Level);
        }
        if (Level > 1 && OnLevelUp != null)
        {
            OnLevelUp(Level);
        }
    }

    private int SetPointsAvaiableForAbilities()
    {
        Ability[] characterAbilities = character.CharacterAbilityManager.CharacterAbilities;
        float currentMaxLevel;
        if (Level == 1 || Level == 3 || Level == 5 || Level == 7 || Level == 9)//Level == 11 is for champions that have abilities with a MaxLevel of 6 (ex. RyzeQ), have to check that
        {
            currentMaxLevel = (Level + 1) * 0.5f;
            pointsAvailableForQ += CanAddAbilityPointToNormalAbility(characterAbilities[0], currentMaxLevel) ? 1 : 0;
            pointsAvailableForW += CanAddAbilityPointToNormalAbility(characterAbilities[1], currentMaxLevel) ? 1 : 0;
            pointsAvailableForE += CanAddAbilityPointToNormalAbility(characterAbilities[2], currentMaxLevel) ? 1 : 0;
            pointsAvailableForR += CanAddAbilityPointToNormalAbility(characterAbilities[3], currentMaxLevel) ? 1 : 0;//Some champions don't have ultimates (ex. Udyr)
        }
        else if (Level == 6 || Level == 11 || Level == 16)
        {
            currentMaxLevel = (Level - 1) * 0.2f;
            pointsAvailableForR += CanAddAbilityPointToUltimateAbility(characterAbilities[3], currentMaxLevel) ? 1 : 0;
        }

        return pointsAvailableForQ + pointsAvailableForW + pointsAvailableForE + pointsAvailableForR;
    }

    private bool CanAddAbilityPointToNormalAbility(Ability ability, float currentMaxLevel)
    {
        return !ability.IsAnUltimateAbility && ability.MaxLevel >= currentMaxLevel;
    }

    private bool CanAddAbilityPointToUltimateAbility(Ability ability, float currentMaxLevel)
    {
        return ability.IsAnUltimateAbility && ability.MaxLevel >= currentMaxLevel;
    }

    private void OnAbilityLevelUp(int abilityId)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_AbilityLevelUp(abilityId);
        }
        else
        {
            LevelUpAbility(abilityId);
        }
    }

    private void SendToServer_AbilityLevelUp(int abilityId)
    {
        character.PhotonView.RPC("ReceiveFromServer_AbilityLevelUp", PhotonTargets.AllViaServer, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_AbilityLevelUp(int abilityId)
    {
        LevelUpAbility(abilityId);
    }

    private void LevelUpAbility(int abilityId)
    {
        if (AbilityPoints > 0)
        {
            if (CanLevelUpAbility(abilityId, 0, pointsAvailableForQ))
            {
                pointsAvailableForQ--;
                AbilityLevelUpActions(abilityId);
            }
            else if (CanLevelUpAbility(abilityId, 1, pointsAvailableForW))
            {
                pointsAvailableForW--;
                AbilityLevelUpActions(abilityId);
            }
            else if (CanLevelUpAbility(abilityId, 2, pointsAvailableForE))
            {
                pointsAvailableForE--;
                AbilityLevelUpActions(abilityId);
            }
            else if (CanLevelUpAbility(abilityId, 3, pointsAvailableForR))
            {
                pointsAvailableForR--;
                AbilityLevelUpActions(abilityId);
            }

            if (character.AbilityLevelUpUIManager)
            {
                character.AbilityLevelUpUIManager.SetAbilityPoints(AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
            }
        }
    }

    private bool CanLevelUpAbility(int abilityId, int requiredAbilityId, int pointsAvailableForAbility)
    {
        return abilityId == requiredAbilityId && pointsAvailableForAbility > 0;
    }

    private void AbilityLevelUpActions(int abilityId)
    {
        AbilityPoints--;
        character.CharacterAbilityManager.LevelUpAbility(AbilityCategory.CharacterAbility, abilityId);
    }

    public void ReachMaxLevel()
    {
        if (!StaticObjects.OnlineMode)
        {
            while (Level < MaxLevel)
            {
                PrepareLevelUp();
            }
            Ability[] characterAbilities = character.CharacterAbilityManager.CharacterAbilities;
            for (int i = 0; i < characterAbilities.Length; i++)
            {
                for (int j = 0; j < characterAbilities[i].MaxLevel; j++)
                {
                    OnAbilityLevelUp(i);
                }
            }
        }
    }
}

using Photon.Pun;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Champion champion;

    public int Level { get; private set; }

    private const int MAX_LEVEL = 18;

    private int abilityPoints;
    private int pointsAvailableForQ;
    private int pointsAvailableForW;
    private int pointsAvailableForE;
    private int pointsAvailableForR;

    public delegate void OnLevelUpHandler(int level);
    public event OnLevelUpHandler OnLevelUp;

    private void Awake()
    {
        champion = GetComponent<Champion>();
    }

    private void Start()
    {
        if (!champion.AbilityLevelUpUIManager) return;

        champion.AbilityLevelUpUIManager.OnAbilityLevelUp += OnAbilityLevelUp;
        champion.InputManager.OnAbilityLevelUp += OnAbilityLevelUp;
        PrepareLevelUp();
    }

    public void PrepareLevelUp()
    {
        if (Level >= MAX_LEVEL) return;

        if (StaticObjects.OnlineMode)
        {
            SendToServer_LevelUp();
        }
        else
        {
            LevelUp();
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
        champion.PhotonView.RPC(nameof(ReceiveFromServer_LevelUp), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void ReceiveFromServer_LevelUp()
    {
        LevelUp();
    }

    private void LevelUp()
    {
        ++Level;
        if (SetPointsAvailableForAbilities() > abilityPoints)
        {
            ++abilityPoints;
        }

        if (champion.AbilityLevelUpUIManager && abilityPoints > 0)
        {
            champion.AbilityLevelUpUIManager.gameObject.SetActive(true);
            champion.AbilityLevelUpUIManager.SetAbilityPoints(abilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
        }

        if (champion.LevelUIManager)
        {
            champion.LevelUIManager.SetLevel(Level);
        }

        if (Level > 1)
        {
            OnLevelUp?.Invoke(Level);
        }
    }

    private int SetPointsAvailableForAbilities()
    {
        Ability[] characterAbilities = champion.AbilityManager.CharacterAbilities;
        float currentMaxLevel;
        if (Level == 1 || Level == 3 || Level == 5 || Level == 7 || Level == 9
        ) //Level == 11 is for champions that have abilities with a MaxLevel of 6 (ex. RyzeQ), have to check that
        {
            currentMaxLevel = (Level + 1) * 0.5f;
            pointsAvailableForQ += CanAddAbilityPointToNormalAbility(characterAbilities[0], currentMaxLevel) ? 1 : 0;
            pointsAvailableForW += CanAddAbilityPointToNormalAbility(characterAbilities[1], currentMaxLevel) ? 1 : 0;
            pointsAvailableForE += CanAddAbilityPointToNormalAbility(characterAbilities[2], currentMaxLevel) ? 1 : 0;
            pointsAvailableForR += CanAddAbilityPointToNormalAbility(characterAbilities[3], currentMaxLevel) ? 1 : 0; //Some champions don't have ultimates (ex. Udyr)
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
        champion.PhotonView.RPC(nameof(ReceiveFromServer_AbilityLevelUp), RpcTarget.AllViaServer, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_AbilityLevelUp(int abilityId)
    {
        LevelUpAbility(abilityId);
    }

    private void LevelUpAbility(int abilityId)
    {
        if (abilityPoints <= 0) return;

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

        if (champion.AbilityLevelUpUIManager)
        {
            champion.AbilityLevelUpUIManager.SetAbilityPoints(abilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
        }
    }

    private bool CanLevelUpAbility(int abilityId, int requiredAbilityId, int pointsAvailableForAbility)
    {
        return abilityId == requiredAbilityId && pointsAvailableForAbility > 0;
    }

    private void AbilityLevelUpActions(int abilityId)
    {
        abilityPoints--;
        champion.AbilityManager.LevelUpAbility(AbilityCategory.CharacterAbility, abilityId);
    }

    public void ReachMaxLevel()
    {
        if (StaticObjects.OnlineMode) return;

        while (Level < MAX_LEVEL)
        {
            PrepareLevelUp();
        }

        Ability[] characterAbilities = champion.AbilityManager.CharacterAbilities;
        for (int i = 0; i < characterAbilities.Length; i++)
        {
            for (int j = 0; j < characterAbilities[i].MaxLevel; j++)
            {
                OnAbilityLevelUp(i);
            }
        }
    }
}

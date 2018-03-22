using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private Character character;

    public int AbilityPoints { get; private set; }
    public int Level { get; private set; }

    private const int MAX_LEVEL = 18;

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
            character.CharacterInput.OnAbilityLevelUp += OnAbilityLevelUp;
            PrepareLevelUp();
        }
    }

    public void PrepareLevelUp()
    {
        if (Level < MAX_LEVEL)
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
        SetPointsAvaiableForAbilities();
        ++AbilityPoints;
        if (character.AbilityLevelUpUIManager)
        {
            character.AbilityLevelUpUIManager.gameObject.SetActive(true);
            character.AbilityLevelUpUIManager.SetAbilityPoints(AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
            character.LevelUIManager.SetLevel(Level);
        }
        if (Level > 1 && OnLevelUp != null)
        {
            OnLevelUp(Level);
        }
    }

    private void SetPointsAvaiableForAbilities()
    {
        if (Level == 1 || Level == 3 || Level == 5 || Level == 7 || Level == 9)
        {
            pointsAvailableForQ++;
            pointsAvailableForW++;
            pointsAvailableForE++;
        }
        else if (Level == 6 || Level == 11 || Level == 16)
        {
            pointsAvailableForR++;
        }
    }

    private void OnAbilityLevelUp(int abilityId)
    {
        if (StaticObjects.OnlineMode)
        {
            SendToServer_AbilityLevelUp(abilityId);
        }
        else
        {
            AbilityLevelUp(abilityId);
        }
    }

    private void SendToServer_AbilityLevelUp(int abilityId)
    {
        character.PhotonView.RPC("ReceiveFromServer_AbilityLevelUp", PhotonTargets.AllViaServer, abilityId);
    }

    [PunRPC]
    private void ReceiveFromServer_AbilityLevelUp(int abilityId)
    {
        AbilityLevelUp(abilityId);
    }

    private void AbilityLevelUp(int abilityId)
    {
        if (AbilityPoints > 0)
        {
            if (abilityId == 0 && pointsAvailableForQ > 0)
            {
                AbilityPoints--;
                pointsAvailableForQ--;
                character.CharacterAbilityManager.LevelUpAbility(AbilityInput.Q);
            }
            else if (abilityId == 1 && pointsAvailableForW > 0)
            {
                AbilityPoints--;
                pointsAvailableForW--;
                character.CharacterAbilityManager.LevelUpAbility(AbilityInput.W);
            }
            else if (abilityId == 2 && pointsAvailableForE > 0)
            {
                AbilityPoints--;
                pointsAvailableForE--;
                character.CharacterAbilityManager.LevelUpAbility(AbilityInput.E);
            }
            else if (abilityId == 3 && pointsAvailableForR > 0)
            {
                AbilityPoints--;
                pointsAvailableForR--;
                character.CharacterAbilityManager.LevelUpAbility(AbilityInput.R);
            }

            if (character.AbilityLevelUpUIManager)
            {
                character.AbilityLevelUpUIManager.SetAbilityPoints(AbilityPoints, pointsAvailableForQ, pointsAvailableForW, pointsAvailableForE, pointsAvailableForR);
            }
        }
    }

    public void ReachMaxLevel()
    {
        if (!StaticObjects.OnlineMode)
        {
            while (Level < MAX_LEVEL)
            {
                PrepareLevelUp();
            }
            for (int i = 0; i < 5; i++)
            {
                OnAbilityLevelUp(0);
                OnAbilityLevelUp(1);
                OnAbilityLevelUp(2);
                if (i < 3)
                {
                    OnAbilityLevelUp(3);
                }
            }
        }
    }
}

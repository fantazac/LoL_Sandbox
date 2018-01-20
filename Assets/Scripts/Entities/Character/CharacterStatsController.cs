using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    private CharacterStats characterStats;
    private bool showStats;

    private void OnEnable()
    {
        characterStats = GetComponent<CharacterStats>();
        showStats = !StaticObjects.OnlineMode || (StaticObjects.PhotonView != null && StaticObjects.PhotonView.isMine);
    }

    public float GetCurrentMovementSpeed()
    {
        return characterStats.MovementSpeed.GetCurrentMovementSpeedForMovement();
    }

    private void OnGUI()
    {
        if (showStats)
        {
            if (StaticObjects.OnlineMode)
            {
                GUILayout.Label("");//ping goes there in online
            }
            GUILayout.Label(characterStats.Health.GetUIText());
            GUILayout.Label(characterStats.Resource.GetUIText());

            GUILayout.Label(characterStats.AttackDamage.GetUIText());
            GUILayout.Label(characterStats.AbilityPower.GetUIText());
            GUILayout.Label(characterStats.Armor.GetUIText());
            GUILayout.Label(characterStats.MagicResistance.GetUIText());
            GUILayout.Label(characterStats.AttackSpeed.GetUIText());
            GUILayout.Label(characterStats.CooldownReduction.GetUIText());
            GUILayout.Label(characterStats.CriticalStrikeChance.GetUIText());
            GUILayout.Label(characterStats.MovementSpeed.GetUIText());

            GUILayout.Label(characterStats.HealthRegenaration.GetUIText());
            GUILayout.Label(characterStats.ResourceRegeneration.GetUIText());
            GUILayout.Label(characterStats.ArmorPenetration.GetUIText());
            GUILayout.Label(characterStats.MagicPenetration.GetUIText());
            GUILayout.Label(characterStats.AttackRange.GetUIText());
            GUILayout.Label(characterStats.LifeSteal.GetUIText());
            GUILayout.Label(characterStats.SpellVamp.GetUIText());
            GUILayout.Label(characterStats.Tenacity.GetUIText());

            GUILayout.Label("POSITION: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
            GUILayout.Label("ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ", " + transform.rotation.w);
        }
    }
}

using UnityEngine;
using System.Collections;

public class SkillShot : Ability
{
    [SerializeField]
    protected GameObject projectilePrefab;

    protected float range = 1000;
    protected float speed = 1000;
    protected float damage = 100;
    protected RaycastHit hit;

    protected override void Start()
    {
        ModifyValues();
        base.Start();
    }

    protected bool CanUseSkill(Vector3 mousePosition)
    {
        return MousePositionOnTerrain.GetRaycastHit(mousePosition, out hit); //&& playerMovement.Player.CanCastSpell(this);
    }

    protected void ModifyValues()
    {
        range /= StaticObjects.DivisionFactor;
        speed /= StaticObjects.DivisionFactor;
    }

    public override void OnPressedInput(Vector3 mousePosition)
    {
        if (CanUseSkill(mousePosition))
        {
            if (StaticObjects.OnlineMode)
            {
                SendToServer_Ability_SkillShot(hit.point + character.CharacterMovement.CharacterHeightOffset);
            }
            else
            {
                UseAbility(hit.point + character.CharacterMovement.CharacterHeightOffset); 
            }
        }
    }

    [PunRPC]
    protected void ReceiveFromServer_Ability_SkillShot(Vector3 destination)
    {
        UseAbility(destination);
    }

    protected void SendToServer_Ability_SkillShot(Vector3 destination)
    {
        character.PhotonView.RPC("ReceiveFromServer_Ability_SkillShot", PhotonTargets.AllViaServer, destination);
    }

    protected override void UseAbility(Vector3 destination)
    {
        character.CharacterOrientation.RotateCharacterInstantly(destination);
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<ProjectileMovement>().ShootProjectile(GetComponent<Health>(), speed, range, damage);//HEALTH TEMPORAIRE
    }
}

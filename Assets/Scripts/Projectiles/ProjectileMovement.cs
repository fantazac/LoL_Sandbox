using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileMovement : MonoBehaviour
{
    private float speed;
    private float range;
    private float damage;
    private Health healthOfShooter;//TEMPORAIRE

    private Vector3 initialPosition;

    public void ShootProjectile(Health healthOfShooter, float speed, float range, float damage)
    {
        this.speed = speed;
        this.range = range;
        this.damage = damage;
        this.healthOfShooter = healthOfShooter;
        initialPosition = transform.position;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (Vector3.Distance(transform.position, initialPosition) < range)
        {
            transform.position += transform.forward * Time.deltaTime * speed;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        Health targetHealth = collider.gameObject.GetComponent<Health>();

        if (targetHealth != null && healthOfShooter != targetHealth)
        {
            targetHealth.Hit(damage);
            Destroy(gameObject);
        }
    }
}

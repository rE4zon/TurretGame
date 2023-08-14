using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private float detectionRadius = 10.0f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireRate = 2.0f;
    [SerializeField] private float projectileSpeed = 10.0f;

    private Transform target;
    private Coroutine firingCoroutine;


    private void Update()
    {
        MonsterDetected();
        TurretRotation();
    }

    private void MonsterDetected()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Monster"))
            {
                target = collider.transform;
                break;
            }
        }
    }

    private void TurretRotation()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0.0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1.0f)
            {
                StartFiring();
            }
            else
            {
                StopFiring();
            }
        }
        else
        {
            StopFiring();
        }
    }

    private void StartFiring()
    {
        if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireCoroutine());
        }
    }

    private void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (target != null)
        {
            Fire();
            yield return new WaitForSeconds(1.0f / fireRate);
        }
    }

    private void Fire()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 targetDirection = target.position - projectile.transform.position;
            projectile.transform.rotation = Quaternion.LookRotation(targetDirection);

            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            Destroy(projectile, 2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

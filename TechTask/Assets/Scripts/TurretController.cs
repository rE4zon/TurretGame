using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5.0f; 
    [SerializeField] private float detectionRadius = 10.0f; 
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform projectileSpawnPoint; 
    [SerializeField] private float fireRate = 2.0f; 
    [SerializeField] private float projectileSpeed = 10.0f; 
    private float lastFireTime; 

    private Transform target; 
    private bool isFiring = false; 


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
            if (collider.CompareTag("Player"))
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
                if (!isFiring)
                {
                    StartFiring();
                }
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
        isFiring = true;
        Fire();
        InvokeRepeating(nameof(Fire), 1.0f / fireRate, 1.0f / fireRate);
    }

    private void StopFiring()
    {
        isFiring = false;
        CancelInvoke(nameof(Fire));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
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

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>()); // Игнорируем столкновение с турелью

            projectile.AddComponent<ProjectileCollisionHandler>();

            Destroy(projectile, 2f);
        }
    }
}

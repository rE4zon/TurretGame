using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float rotationSpeed = 5.0f; // Скорость поворота турели
    public float detectionRadius = 10.0f; // Радиус обнаружения
    public GameObject projectilePrefab; // Префаб снаряда
    public Transform projectileSpawnPoint; // Точка, откуда снаряд будет вылетать
    public float fireRate = 2.0f; // Скорострельность
    public float projectileSpeed = 10.0f; // Скорость снаряда
    private float lastFireTime; // Время последнего выстрела

    private Transform target; // Текущая цель
    private bool isFiring = false; // Флаг для отслеживания состояния стрельбы


    private void Update()
    {
        // Обнаружение цели в радиусе
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            // Проверяем, есть ли у объекта компонент "Player" (замените на свой)
            if (collider.CompareTag("Player"))
            {
                target = collider.transform;
                break;
            }
        }

        // Поворот к цели (если она есть)
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0.0f;

            // Вычисляем поворот только по оси Y
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Плавно поворачиваем турель к цели только по оси Y
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Проверяем, полностью ли турель повернута к цели
            if (Quaternion.Angle(transform.rotation, targetRotation) <= 1.0f)
            {
                // Если турель полностью повернута, и нет активной стрельбы, начинаем стрелять
                if (!isFiring)
                {
                    StartFiring();
                }
            }
            else
            {
                // Если турель не полностью повернута, прекращаем стрельбу
                StopFiring();
            }
        }
        else
        {
            // Если цели нет, прекращаем стрельбу
            StopFiring();
        }
    }

    private void StartFiring()
    {
        isFiring = true;
        // Выстрел и задержка между выстрелами
        Fire();
        InvokeRepeating(nameof(Fire), 1.0f / fireRate, 1.0f / fireRate);
    }

    private void StopFiring()
    {
        isFiring = false;
        // Прекращаем стрельбу и отменяем повторяющиеся вызовы
        CancelInvoke(nameof(Fire));
    }

    private void OnDrawGizmosSelected()
    {
        // Отрисовка сферы радиуса обнаружения в редакторе
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void Fire()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Создаем снаряд
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            // Наводим снаряд на цель
            Vector3 targetDirection = target.position - projectile.transform.position;
            projectile.transform.rotation = Quaternion.LookRotation(targetDirection);

            // Добавляем скорость снаряду
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = targetDirection.normalized * projectileSpeed;
            }

            // Проверяем столкновение снаряда с целью
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>()); // Игнорируем столкновение с турелью

            // Добавляем компонент для обработки столкновения
            projectile.AddComponent<ProjectileCollisionHandler>();

            // Уничтожаем снаряд через некоторое время (чтобы избежать нагрузки на производительность)
            Destroy(projectile, 10.0f);
        }
    }



}

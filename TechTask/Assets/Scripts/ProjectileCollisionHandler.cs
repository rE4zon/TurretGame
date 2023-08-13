using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем столкновение с объектами, имеющими тег "Player" (замените на свой тег, если необходимо)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Уничтожаем цель
            Destroy(collision.gameObject);
            // Уничтожаем снаряд
            Destroy(gameObject);
        }
    }
}
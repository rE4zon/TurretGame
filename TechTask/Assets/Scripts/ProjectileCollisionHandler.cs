using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
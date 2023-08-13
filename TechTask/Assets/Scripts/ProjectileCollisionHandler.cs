using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // ��������� ������������ � ���������, �������� ��� "Player" (�������� �� ���� ���, ���� ����������)
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���������� ����
            Destroy(collision.gameObject);
            // ���������� ������
            Destroy(gameObject);
        }
    }
}
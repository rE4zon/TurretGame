using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5.0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MonsterSpawn();
        }
    }

    private void MonsterSpawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;

            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

            StartCoroutine(MoveMonsterToTarget(monster.transform));
        }
    }

    private IEnumerator MoveMonsterToTarget(Transform monsterTransform)
    {
        while (monsterTransform != null && Vector3.Distance(monsterTransform.position, targetObject.position) > 0.01f)
        {
            Vector3 newPosition = Vector3.MoveTowards(monsterTransform.position, targetObject.position, moveSpeed * Time.deltaTime);
            monsterTransform.position = newPosition;
            yield return null;
        }

        if (monsterTransform != null)
        {
            Destroy(monsterTransform.gameObject);
        }
    }
}



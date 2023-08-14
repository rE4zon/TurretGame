using UnityEngine;
using System.Collections;
using TMPro;


public class MonsterController : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Transform targetObject;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text turretLifeTimeText;
    [SerializeField] private TMP_Text healthBar;



    private int currentHealth = 3;
    private int reachedTargetCount = 0;
    private bool isGameOver = false;
    private float monsterLifeTime = 0f;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void Update()
    {
        if (!isGameOver && Input.GetMouseButtonDown(0))
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
        while (monsterTransform != null && targetObject != null && Vector3.Distance(monsterTransform.position, targetObject.position) > 0.01f)
        {
            Vector3 newPosition = Vector3.MoveTowards(monsterTransform.position, targetObject.position, moveSpeed * Time.deltaTime);
            monsterTransform.position = newPosition;
            yield return null;
        }

        if (monsterTransform != null && targetObject != null)
        {
            reachedTargetCount++;

            monsterLifeTime = timer.GetElapsedTime();

            GameOverPanel();

            Destroy(monsterTransform.gameObject);
        }
    }

    private void GameOverPanel()
    {
        if (reachedTargetCount >= 3 && !isGameOver)
        {
            isGameOver = true;
            if (gameOverMenu != null)
            {
                gameOverMenu.SetActive(true);
            }

            if (targetObject != null)
            {
                Destroy(targetObject.gameObject);
            }

            if (timer != null)
            {
                timer.StopTimer();
            }

            Time.timeScale = 0f;

            if (turretLifeTimeText != null)
            {
                turretLifeTimeText.text = "Turret Life Time: " + monsterLifeTime.ToString();
            }
        }
        if (currentHealth > 0)
        {
            currentHealth--;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.text = "" + currentHealth.ToString();
        }
    }
}
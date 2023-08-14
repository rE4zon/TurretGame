using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    private float startTime;
    private bool timeIsRunning = true;

    private void Start()
    {
        startTime = Time.time;
    }
    private void Update()
    {
        if (timeIsRunning)
        {
            float timeElapsed = Time.time - startTime;
            DisplayTime(timeElapsed);
        }
    }
    public void StopTimer()
    {
        timeIsRunning = false;
    }
    public float GetElapsedTime()
    {
        return Time.time - startTime;
    }
    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}

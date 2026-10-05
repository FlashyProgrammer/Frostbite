using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    [Header("Time Parameters")]
    [SerializeField] private float timer;

    private bool timeIsRunning;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Game Master")]
    [SerializeField] private GameMaster gameMaster;


    private void Awake()
    {
        timeIsRunning = true;
    }

    private void Update()
    {

        if (!timeIsRunning) return;

        if (timer > 0)
        {

            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
        }
        else
        {
            timer = 0;
            timeIsRunning = false;
            UpdateTimerDisplay(timer);
            OnTimerEnd();
        }

    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

    
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd()
    {
        gameMaster.DayOver();
    }
}



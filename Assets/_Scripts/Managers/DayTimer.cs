using TMPro;
using UnityEngine;

public class DayTimer : MonoBehaviour
{
    [Header("Time Parameters")]
    [SerializeField] private float timer;

    private bool timeIsRunning;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI roverTimer;

    [Header("Game Master")]
    [SerializeField] private GameMaster gameMaster;

    [Header("Misc")]
    [SerializeField] private GameObject playerRover;


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

        if (playerRover != null)
        {
            if (playerRover.activeInHierarchy)
            {
                timeText.enabled = false;
                roverTimer.enabled = true;
            }

            else
            {
                timeText.enabled = true;
            }

        }


    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);


        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(playerRover != null) roverTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd()
    {
        gameMaster.DayOver();
    }
}



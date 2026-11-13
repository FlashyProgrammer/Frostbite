using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Rendering;

public class GameMaster : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject craftingScreen;
    [SerializeField] private GameObject completeScreen;

    [Header("Game Settings")]
    [SerializeField] private GameObject playerGFX;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private CameraMove mouseLook;
    [SerializeField] private SleepingArea sleepingArea;
    [SerializeField] private List<EnemySpawner> enemySpawners;

    [Header("Cut scenes")]
    [SerializeField] private TimelineAsset deathCutscene;
    private int pauseCounter = 0;


    private void Update()
    {
        if (craftingScreen.activeInHierarchy)
        {
            playerMovement.enabled = false;
        }

        if (pauseMenuUI.activeInHierarchy)
        {
            playerMovement.enabled = true;
        }
    }
    private void Awake()
    {
        playerMovement.enabled = true;

    }
    public void GameOver()
    {
        gameOverUI.SetActive(true);
        FreezePlayer();
        Cursor.lockState = CursorLockMode.None;
        foreach (var spawner in enemySpawners)
        {
            foreach (var enemy in spawner.activeEnemies)
            {
                enemy.SetActive(false);
            }

            spawner.activeEnemies.Clear();
            spawner.enabled = false;
        }
        if (TryGetComponent<PlayableDirector>(out PlayableDirector playableDirector))
        {
            playableDirector.playableAsset = deathCutscene;
            playableDirector.time = 0;
            playableDirector.Play();
        }

    }
    public void DayOver()
    {
       
        sleepingArea.isCompleted = true;
        sleepingArea.canSleep = true;
        foreach (var spawner in enemySpawners)
        {
            foreach (var enemy in spawner.activeEnemies)
            {
                enemy.SetActive(false);
            }

            spawner.activeEnemies.Clear();
            spawner.enabled = false;
        }

    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && pauseCounter == 0 && playerMovement.enabled)
        {
            pauseMenuUI.SetActive(true);
            playerInteraction.enabled = false;
            playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            pauseCounter++;
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }

        if (context.canceled && pauseCounter == 1)
        {
            pauseCounter++;
        }

        if (context.performed && pauseCounter == 2)
        {

            pauseMenuUI.SetActive(false);
            playerMovement.enabled = true;
            playerInteraction.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            AudioListener.pause = false;
            pauseCounter = 0;
        }

    }

    public void NextDay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CompleteMenu()
    {
        FreezePlayer();
        completeScreen.SetActive(true);

    }
    public void RemoveComplete()
    {
        UnfreezePlayer();
        completeScreen.SetActive(false);
    }
    public void FreezePlayer()
    {
        playerGFX.SetActive(false);
        playerMovement.enabled = false;
        playerInteraction.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        mouseLook.enabled = false;
    }
    public void UnfreezePlayer()
    {
        playerGFX.SetActive(true);
        playerMovement.enabled = true;
        playerInteraction.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        mouseLook.enabled = true;
    }

    public void StopAudio()
    {
        AudioListener.pause = true;
    }
}

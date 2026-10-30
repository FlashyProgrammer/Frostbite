using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject craftingScreen;

    [Header("Game Settings")]
    [SerializeField] private PlayerMovement playerMovement;
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
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        AudioListener.pause = true;

    }
    public void DayOver()
    {
        winScreenUI.SetActive(true);
        playerMovement.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true;
        Time.timeScale = 0f;


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

        if(context.performed && pauseCounter == 2)
        {
          
            pauseMenuUI.SetActive(false);
            playerMovement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            AudioListener.pause = false;
            pauseCounter = 0;                                  
        }
       
    }
}

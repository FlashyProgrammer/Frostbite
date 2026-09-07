using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Game Settings")]
    [SerializeField] private PlayerMovement playerMovement;
    private int pauseCounter = 0;

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

    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed && pauseCounter == 0)
        {
            pauseMenuUI.SetActive(true);
            playerMovement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            pauseCounter++;
            Time.timeScale = 0f;
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
            pauseCounter = 0;                                  
        }
       
    }
}

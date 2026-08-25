using System;
using System.ComponentModel.Design;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI;

    [Header("Game Settings")]
    [SerializeField] private PlayerMovement playerMovement;
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
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

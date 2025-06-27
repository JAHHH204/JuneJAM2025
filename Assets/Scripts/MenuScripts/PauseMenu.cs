using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu UI Elements")]
    public Canvas pauseMenuCanvas;
    public Canvas settingsCanvas;
    public GameObject player;
    [Header("Menu States")]
    public bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.enabled = false;
        settingsCanvas.enabled = false;
       // settingsCanvas.GetComponentInChildren<UnityEngine.UI.Slider>().value = GameManager.instance.volume;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Stop the game time
        isPaused = true;
        pauseMenuCanvas.enabled = true; // Show pause menu
        player.GetComponent<PlayerController>().enabled = false; // Disable player controls
        Debug.Log("Game Paused");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game time
        isPaused = false;
        pauseMenuCanvas.enabled = false; // Hide pause menu
        player.GetComponent<PlayerController>().enabled = true; // Enable player controls
        Debug.Log("Game Resumed");
    }
    public void OpenSettings()
    {
        pauseMenuCanvas.enabled = false; // Hide pause menu
        settingsCanvas.enabled = true; // Show settings menu
        Debug.Log("Opening Settings");
    }
    public void CloseSettings()
    {
        settingsCanvas.enabled = false; // Hide settings menu
        pauseMenuCanvas.enabled = true; // Show pause menu
        Debug.Log("Closing Settings");
    }
    public void Return()
    {
        SceneManager.LoadScene("MainMenu"); // Return to main menu
    }
    public void AdjustVolume()
    {
       // GameManager.instance.volume = settingsCanvas.GetComponentInChildren<UnityEngine.UI.Slider>().value;
    }
}

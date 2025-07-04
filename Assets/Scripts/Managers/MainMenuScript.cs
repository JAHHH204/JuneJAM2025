using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Main Menu UI Elements")]
    public Canvas mainMenuCanvas;
    public Canvas settingsCanvas;

    [Header("Menu States")]
    public bool isSettingsOpen = false;
    void Start()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.enabled = false; // Ensure settings canvas is hidden at start
        }
    }

    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("TutorialLevel");
        Debug.Log("Starting the game");
    }

    public void Settings()
    {
        if (isSettingsOpen)
        {
            mainMenuCanvas.enabled = true;
            settingsCanvas.enabled = false;
            isSettingsOpen = false;
            Debug.Log("Closing Settings");
        }
        else
        {
            mainMenuCanvas.enabled = false;
            settingsCanvas.enabled = true;
            isSettingsOpen = true;
            Debug.Log("Opening Settings");
        }
    }
    public void QuitGame()
    {
        Debug.Log("Game is quitting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else

    Application.Quit();
#endif
    }
    public void AdjustVolume()
    {
        GameManager.instance.volume = settingsCanvas.GetComponentInChildren<UnityEngine.UI.Slider>().value;
    }
}

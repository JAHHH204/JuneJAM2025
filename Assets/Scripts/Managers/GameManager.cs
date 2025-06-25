using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Game Settings")]
    public float volume = 0.5f;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioListener.volume != volume)
        {
            UpdateVolume();
        }
    }
    public void UpdateVolume()
    {
        // Assuming you have an AudioSource component to control volume
        AudioListener.volume = volume; // Set the global audio volume
        Debug.Log("Volume updated to: " + volume);
    }
}

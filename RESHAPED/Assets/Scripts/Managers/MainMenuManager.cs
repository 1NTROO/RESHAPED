using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance; // Singleton instance of MainMenuManager
    public static MainMenuManager Instance { get { return instance; } } // Public property to access the instance

    private void Awake()
    {
        if (instance != null && instance != this) // Check if an instance already exists
        {
            Destroy(gameObject); // Destroy the duplicate instance
        }
        else
        {
            instance = this; // Set the current instance as the singleton instance
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
        Debug.Log("Game has been quit!"); // Log a message indicating the game has been quit
    }
}

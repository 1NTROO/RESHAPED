using UnityEngine;

public class PauseManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void ContinueGame()
    {
        SkillTreeManager.Instance.PauseGame();
    }

    public void ResetGame()
    {
        Time.timeScale = 1f; // Reset the time scale to normal
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene"); // Load the main menu scene
    }
}

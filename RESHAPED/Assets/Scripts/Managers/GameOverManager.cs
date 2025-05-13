using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI finalXPText; // Reference to the game over text UI element

    void Start()
    {
        finalXPText.text = "Total XP: " + KeepInfoOnLoad.Instance.finalXP.ToString(); // Display the final XP value
    }

    void Update()
    {
        
    }

    public void RestartGame()
    {
        KeepInfoOnLoad.Instance.finalXP = 0; // Store the current XP value
        SceneManager.LoadScene("MainGameScene");
    }
}

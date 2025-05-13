using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepInfoOnLoad : MonoBehaviour
{
    // singleton for easy access throughout the whole project
    private static KeepInfoOnLoad instance;
    public static KeepInfoOnLoad Instance { get { return instance; } }

    public float finalXP;

    void Awake()
    {
        // setup singleton
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void KeepInfo(float xp)
    {
        finalXP = xp; // Store the final XP value
    }
}

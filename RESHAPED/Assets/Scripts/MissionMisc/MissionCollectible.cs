using UnityEngine;

public class MissionCollectible : MonoBehaviour
{
    [SerializeField] AudioClip collectSound;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play the collection sound
            AudioManager.Instance.PlayClip(collectSound);
            // Check if the player is on a mission
            if (MissionManager.Instance.isMissionActive && MissionManager.Instance.activeMissionType == MissionManager.MissionType.Collect)
            {
                // Increment the mission progress
                MissionManager.Instance.ProgressMission(1);

                // Destroy the collectible object
                Destroy(gameObject);
            }
        }
    }
}

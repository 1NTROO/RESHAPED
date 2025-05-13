using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] 
    private Transform player;

    [SerializeField] 
    private int cameraDistance; // distance the camera should maintain from the player
    void Start()
    {
        
    }

    void Update()
    {
        // sets the transform of the camera to follow the player with a set distance.
        if (player != null)
            transform.position = player.transform.position + new Vector3(0, 0, -cameraDistance);
    }
}

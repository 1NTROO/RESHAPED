using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private static Shoot instance; // Singleton instance of the Shoot class
    public static Shoot Instance { get { return instance;  }} // Singleton instance of the Shoot class
    
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

    [Header("Objects")]
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform firePoint; // Point from where the bullet will be fired

    [Header("Settings")]
    public float projectilesPerShot = 1; // Number of projectiles fired per shot
    public float burstFireInterval = 0.15f;
    private bool burstFire = false; // Whether the weapon should fire in bursts
    public float spread = 1f; // Spread of the bullets when fired
    private float spreadAngle = 30; // Angle of spread for the bullets
    public bool explodeOnImpact = false; // Whether the bullet should explode on impact
    public float bulletSpeed = 1f; // Speed of the bullet
    public float damageModifier = 1f;
    private float fireRate = 0.5f; // Rate at which the weapon can fire (in seconds)
    private float nextFireTime = 0f; // Time when the next bullet can be fired

    [Header("Damage Settings")]
    public float damage = 10f; // Damage dealt by the bullet

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Check if the left mouse button is pressed
        {
            FireWeapon(); // Call the method to fire the weapon
        }
    }

    void FireWeapon()
    {
        firePoint = transform;
        if (Time.time >= nextFireTime) // Check if the current time is greater than or equal to the next fire time
        {
            fireRate = PlayerStats.Instance.cooldownTotal / 1000; // Get the cooldown time from the PlayerStats singleton instance

            nextFireTime = Time.time + fireRate; // Set the next fire time based on the fire rate

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world coordinates

            StartCoroutine(BurstFireInterval(burstFireInterval, mousePos)); // Start the coroutine for burst fire


            PlayerStats.Instance.OnFire();
        }
    }

    void PassBulletStats(GameObject b)
    {
        BulletHandler bulletHandler = b.GetComponent<BulletHandler>(); // Get the BulletHandler component from the bullet prefab
        if (bulletHandler != null)
        {
            damage = PlayerStats.Instance.damageTotal; // Get the total damage from the PlayerStats singleton instance
            damage *= damageModifier; // Apply the damage modifier to the bullet's damage
            bulletHandler.damage = damage; // Set the damage of the bullet
        }
    }

    public IEnumerator BurstFireInterval(float burstInterval, Vector2 mousePos)
    {
        for (int i = 0; i < projectilesPerShot; i++) // Loop to fire multiple projectiles
        {
            for (int j = 1; j <= spread; j++) // Loop to apply spread to the bullets
            {
                Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized; // Calculate the direction from the fire point to the mouse position

                print("Applying Spread"); // Debug message to indicate spread is being applied
                float newSpreadAngle = Random.Range(-spreadAngle, spreadAngle); // Calculate a random spread angle for the bullet
                if (j == 1) { newSpreadAngle = 0; } // If spread is 1, set the new spread angle to 0
                print(newSpreadAngle); // Debug message to show the new spread angle
                Quaternion bulletRotation = Quaternion.Euler(0, 0, newSpreadAngle); // Create a rotation for the bullet based on the spread angle

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation); // Create a new bullet instance with the modified rotation

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the bullet

                PassBulletStats(bullet); // Call the method to pass stats to the bullet

                fireDirection = bulletRotation * fireDirection; // Apply the rotation to the fire direction
                rb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse); // Apply force to the bullet in the direction of the fire point's up vector
            }

            yield return new WaitForSeconds(burstInterval); // Wait for the specified burst interval before allowing the next burst fire
        }
    }

    public void SetWeaponStats(string type, float value, string secondaryType = null, float secondaryValue = 0f)
    // Set the weapon stats based on the type and value provide
    {
        switch (type)
        {
            case "projectilesPerShot":
                projectilesPerShot += value; // Set the number of projectiles per shot
                break;
            case "burstFireInterval":
                burstFireInterval += value; // Set the burst fire interval
                break;
            case "spread":
                spread += value; // Set the spread of the bullets
                break;
            case "bulletSpeed":
                bulletSpeed *= 1 + value; // Set the speed of the bullet
                break;
            case "damageModifier":
                damageModifier += value; // Set the damage modifier for the bullet
                break;
            case "spreadAngle":
                spreadAngle += value; // Set the angle of spread for the bullets
                break;
            default:
                Debug.LogWarning("Invalid weapon stat type: " + type); // Log a warning for invalid weapon stat type
                break;
        }
        if (secondaryType == null || secondaryValue == 0f) return; // If no secondary type or value is provided, exit the method
        switch (secondaryType)
        {
            case "projectilesPerShot":
                projectilesPerShot += value; // Set the number of projectiles per shot
                break;
            case "burstFireInterval":
                burstFireInterval += value; // Set the burst fire interval
                break;
            case "spread":
                spread += value; // Set the spread of the bullets
                break;
            case "bulletSpeed":
                bulletSpeed += value; // Set the speed of the bullet
                break;
            case "damageModifier":
                damageModifier += value; // Set the damage modifier for the bullet
                break;
            case "spreadAngle":
                spreadAngle += value; // Set the angle of spread for the bullets
                break;
            default:
                Debug.LogWarning("Invalid weapon stat type: " + type); // Log a warning for invalid weapon stat type
                break;

        }
    }
}

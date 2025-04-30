using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Objects")]
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform firePoint; // Point from where the bullet will be fired

    [Header("Settings")]
    public float bulletSpeed = 20f; // Speed of the bullet
    public float fireRate = 0.5f; // Rate of fire (bullets per second)
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
            nextFireTime = Time.time + 1f / fireRate; // Set the next fire time based on the fire rate

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Create a new bullet instance
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the bullet

            PassBulletStats(bullet); // Call the method to pass stats to the bullet

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in world coordinates
            Vector2 fireDirection = (mousePos - (Vector2)firePoint.position).normalized; // Calculate the direction from the fire point to the mouse position

            rb.AddForce(fireDirection * bulletSpeed, ForceMode2D.Impulse); // Apply force to the bullet in the direction of the fire point's up vector
        }
    }

    void PassBulletStats(GameObject b)
    {
        BulletHandler bulletHandler = b.GetComponent<BulletHandler>(); // Get the BulletHandler component from the bullet prefab
        if (bulletHandler != null)
        {
            damage = PlayerStats.Instance.damageTotal; // Get the total damage from the PlayerStats singleton instance
            bulletHandler.damage = damage; // Set the damage of the bullet
        }
    }
}

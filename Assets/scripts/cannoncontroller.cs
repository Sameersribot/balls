using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cannoncontroller : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Unity editor
    public GameObject bulletPrefab;
    public float shootInterval = 1.5f;
    private Vector2 direction;

    private float timeSinceLastShot = 0f;

    void Update()
    {
        if (player == null)
        {
            Debug.Log("Player not assigned to CannonController.");
            return;
        }

        // Rotate cannon towards the player
        direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Shoot bullet every shootInterval seconds
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            Shoot();
            timeSinceLastShot = 0f;
        }
    }

    void Shoot()
    {
        // Instantiate bullet at the cannon's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        // Add any additional logic to the bullet (e.g., setting speed, damage, etc.)
        // bullet.GetComponent<BulletScript>().Initialize(speed, damage); // Example
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 40f);
        // Destroy the bullet after a certain time (adjust as needed)
        Destroy(bullet, 3f);
    }

}

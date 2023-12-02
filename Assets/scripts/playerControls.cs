using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class playerControls : MonoBehaviour
{
    public float carSpeed = 10.0f;
    public GameObject particles;
    public Joystick joystick;
    private float movementx, movementY;
    public float speed, rotateSpeed;
    public float bounceForce = 5f; // Adjust this value to control the bounce force
    private Rigidbody2D rb;
    private Vector2 pose;
    public GameObject[] obstacles;
    public AudioSource glitch;

    void Start()
    {
        pose = transform.position;
        glitch = gameObject.GetComponent<AudioSource>();
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementx = joystick.Horizontal;
        movementY = joystick.Vertical;

        Vector2 movement = new Vector2(movementx, movementY);
        movement.Normalize();

        Move(movement);
        obstacles[0].transform.Rotate(0f, 0f, rotateSpeed);
        obstacles[1].transform.Rotate(0f, 0f, rotateSpeed);
        obstacles[2].transform.Rotate(0f, 0f, rotateSpeed);
        obstacles[3].transform.Rotate(0f, 0f, rotateSpeed);
        
    }

    void Move(Vector2 direction)
    {
        // Apply movement using the Rigidbody2D component
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "obstacle")
        {
            transform.position = pose;
            Instantiate(particles, transform.position, Quaternion.identity);
            glitch.Play();
        }
        else if(collision.gameObject.tag == "finish1")
        {
            transform.position = new Vector2(75.1f, 149.5f);
        }
        else if(collision.gameObject.tag == "finish2")
        {
            transform.position = new Vector2(76.1f, 290f);
        }
        else if(collision.gameObject.tag == "finish3")
        {
            transform.position = new Vector2(76.1f, 347f);
        }
        else if (collision.gameObject.tag == "finish4")
        {
            transform.position = new Vector2(76.1f, 393f);
        }
        else if (collision.gameObject.tag == "finish5")
        {
            transform.position = new Vector2(76.1f, 347f);
        }
        else if(collision.gameObject.tag == "bound")
        {
            ReflectBounce(new Vector2(transform.position.x, transform.position.y - 10f));
        }
    }
    private void ReflectBounce(Vector2 normal)
    {
        transform.position = normal;
    }
}

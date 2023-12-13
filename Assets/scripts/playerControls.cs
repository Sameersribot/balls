using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class playerControls : MonoBehaviour
{
    public float carSpeed = 10.0f;
    public GameObject particles, mainCanvas ,joystickCanvas;
    public Joystick joystick;
    private int lives = 2;
    public GameObject[] heart;
    private float movementx, movementY;
    private Vector2 touchStartPos;
    private float initialY;
    SpriteRenderer sprite;
    TrailRenderer trail;
    public float sensitivity = 0.02f;
    public float speed, rotateSpeed;
    public float bounceForce = 5f; // Adjust this value to control the bounce force
    private Rigidbody2D rb;
    private Vector2 pose;
    private float finalpos, initialpos;
    public GameObject[] obstacles;
    private int skincolor;
    public AudioSource glitch;

    void Start()
    {
        pose = transform.position;
        glitch = gameObject.GetComponent<AudioSource>();
        initialpos = 390f;
        finalpos = 427f;
        skincolor = 4;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        trail = gameObject.GetComponent<TrailRenderer>();
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        //InvokeRepeating("spawner", 2f, 1f);
    }

    void Update()
    {
        rb.velocity = new Vector2(10f, 0f);
        movementx = joystick.Horizontal;
        movementY = joystick.Vertical;
        // Iterate through all the active touches
        Vector2 movement = new Vector2(movementx, movementY);
        movement.Normalize();
        foreach(GameObject obstacle in obstacles)
        {
            obstacle.transform.Rotate(new Vector3(0f, 0f, 5f));
        }
        Move(movement);
    }

    void Move(Vector2 direction)
    {
        // Apply movement using the Rigidbody2D component
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            lives--;
            Destroy(heart[lives]);
            glitch.Play();
            
        }
        else if (collision.gameObject.tag == "finish1")
        {
            transform.position = new Vector2(75.1f, 153f);
        }
        else if (collision.gameObject.tag == "finish2")
        {
            transform.position = new Vector2(76.1f, 290f);
        }
        else if (collision.gameObject.tag == "finish3")
        {
            transform.position = new Vector2(76.1f, 347f);
        }
        else if (collision.gameObject.tag == "finish4")
        {
            transform.position = new Vector2(76.1f, 393f);
        }
        else if (collision.gameObject.tag == "finish5")
        {
            SceneManager.LoadScene("levl2");
        }

    }
    private void ReflectBounce(Vector2 normal)
    {
        transform.position = normal;
    }
    public void redBtn()
    {
        Color colur = new Color(0.83f, 0.33f, 0.33f);
        sprite.color = colur;
        trail.startColor = colur;
        trail.endColor = Color.white;
        skincolor = 1;
    }
    public void yeloBtn()
    {
        Color colur = new Color(0.8218711f, 0.9622642f, 0.3939836f);
        sprite.color = colur;
        trail.startColor = colur;
        trail.endColor = Color.white;
        skincolor = 2;
    }
    public void greenBtn()
    {
        Color colur = new Color(0.347152f, 0.9339623f, 0.6448716f);
        sprite.color = colur;
        trail.startColor = colur;
        trail.endColor = Color.white;
        skincolor = 3;
    }
    public void bluBtn()
    {
        Color colur = new Color(0.148665f, 0.2221597f, 0.9056604f);
        sprite.color = colur;
        trail.startColor = colur;
        trail.endColor = Color.white;
        skincolor = 4;
    }
    void spawner()
    {
        /*float randomInt = Random.Range(1f, 10f);
        if (randomInt < 2f && randomInt > 1f)
        {
            Instantiate(obstacles[0], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 4f && randomInt > 2f)
        {
            Instantiate(obstacles[1], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 5f && randomInt > 4f)
        {
            Instantiate(obstacles[2], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 6f && randomInt > 5f)
        {
            Instantiate(obstacles[3], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 2f && randomInt > 1f)
        {
            Instantiate(obstacles[4], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 2f && randomInt > 1f)
        {
            Instantiate(obstacles[5], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else if (randomInt < 2f && randomInt > 1f)
        {
            Instantiate(obstacles[6], new Vector3(transform.position.x + 15f, transform.position.y, transform.position.z), Quaternion.identity);
        }*/

    }
    public void clickPause()
    {
        mainCanvas.SetActive(false);
        joystickCanvas.SetActive(true);
    }
    public void clickPlay()
    {
        mainCanvas.SetActive(true);
        joystickCanvas.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
public class playerControls : MonoBehaviour
{
    public float carSpeed = 10.0f;
    public GameObject particles, mainCanvas ,joystickCanvas, volumePost;
    public Joystick joystick;
    private int lives = 2, levl;
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
    public float speedofBall = 5f;
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public Slider slider;
    public float vignetteIncreaseRate = 0.1f;
    private float virgenetteInitial;
    private bool isTouchingScreen = false;
    private PostProcessVolume postProcessVolume;
    private Vignette vignette;

    void Start()
    {
        pose = transform.position;
        initialpos = 390f;
        finalpos = 427f;
        skincolor = 4;
        levl = 0;
        slider.value = 1;
        sprite = gameObject.GetComponent<SpriteRenderer>();
        trail = gameObject.GetComponent<TrailRenderer>();
        
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        postProcessVolume = volumePost.GetComponent<PostProcessVolume>();

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
            virgenetteInitial = vignette.intensity.value;
        }
        else
        {
            Debug.LogError("PostProcessVolume or Vignette component not found.");
        }
        //InvokeRepeating("spawner", 2f, 1f);
    }

    void Update()
    {
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
        power();
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
            spawn();
            lives--;
            Destroy(heart[lives]);
            FindObjectOfType<AudioMnagaer>().Play("restart");
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        else if (collision.gameObject.tag == "finish1")
        {
            transform.position = new Vector2(75.1f, 153f);
            levl = 1;
        }
        else if (collision.gameObject.tag == "finish2")
        {
            transform.position = new Vector2(76.1f, 290f);
            levl = 2;
        }
        else if (collision.gameObject.tag == "finish3")
        {
            transform.position = new Vector2(76.1f, 347f);
            levl = 3;
        }
        else if (collision.gameObject.tag == "finish4")
        {
            transform.position = new Vector2(76.1f, 393f);
            levl = 4;
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
    /*public void redBtn()
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
    }*/
    
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
    void power()
    {
        if (Input.touchCount > 1 && slider.value > 0)
        {
            Touch touch = Input.GetTouch(1);

            if (touch.phase == TouchPhase.Began)
            {
                isTouchingScreen = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isTouchingScreen = false;
            }
        }
        else if (Input.GetButton("Jump") && slider.value > 0)
        {
            isTouchingScreen = true;
        }
        else
        {
            isTouchingScreen = false;
        }

        UpdatePlayerSpeed();
        UpdateVignetteEffect();
    }
    private void UpdatePlayerSpeed()
    {
        if (isTouchingScreen)
        {
            FindObjectOfType<AudioMnagaer>().Play("power");
            rb.velocity = new Vector2(rb.velocity.x * acceleration + Time.deltaTime, rb.velocity.y + acceleration + Time.deltaTime);
            slider.value -= Time.deltaTime * 0.1f;
            // Clamp speed to maxSpeed
        }
        else
        {
            // Reset speed when not touching the screen
            slider.value += Time.deltaTime * 0.1f;

        }

        // Move the player or perform other speed-related actions here
        //transform.Translate(Vector3.forward * speedofBall * Time.deltaTime);
    }

    private void UpdateVignetteEffect()
    {
        if (isTouchingScreen && vignette != null)
        {
            // Clamp vignette intensity to a maximum value if needed
            vignette.intensity.value = Mathf.Min(vignette.intensity.value, 1f);

            // Increase the vignette intensity
            vignette.intensity.value += 0.05f;
        }
        else if(Input.GetButton("Jump") && vignette != null)
        {
            // Clamp vignette intensity to a maximum value if needed
            vignette.intensity.value = Mathf.Min(vignette.intensity.value, 0.5f);

            // Increase the vignette intensity
            vignette.intensity.value += 0.05f;
        }
        else if (vignette != null)
        {
            // Reset vignette intensity when not touching the screen
            vignette.intensity.value = virgenetteInitial;

        }
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void spawn()
    {
        if (levl == 0 && lives > 0)
        {
            transform.position = pose;
        }
        else if (levl == 1 && lives > 0)
        {
            transform.position = new Vector2(75.1f, 153f);
        }
        else if (levl == 2 && lives > 0)
        {
            transform.position = new Vector2(76.1f, 290f);
                    }
        else if (levl == 3 && lives > 0)
        {
            transform.position = new Vector2(76.1f, 347f);
                    }
        else if (levl == 4 && lives > 0)
        {
            transform.position = new Vector2(76.1f, 393f);
        }
        else
        {
            restart();
        }
    }
}

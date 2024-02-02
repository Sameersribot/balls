using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class playerControls : MonoBehaviour
{
    public float carSpeed = 10.0f;
    public GameObject particles, mainCanvas ,joystickCanvas, volumePost, speedingEfct, sprite_boundary;
    public Joystick joystick;
    private int lives = 2, levl;
    //public CinemachineCameraOffset cinemachine;
    public GameObject[] heart;
    private float movementx, movementY;
    private Vector2 currentDirection, touchStartPos, movement;
    private float initialY;
    private SpriteRenderer sprite;
    public Color boundary_color;
    TrailRenderer trail;
    public float sensitivity = 0.02f;
    public float speed, rotationSpeed;
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
    private float virgenetteInitial, bloomInitial, chromaticInitial;
    private bool isTouchingScreen = false;
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    private Bloom bloom;
    private ChromaticAberration chromatic;

    // Intensity of the shake
    public float shakeIntensity = 1f;

    // Duration of the shake in seconds
    public float shakeDuration = 1f;

    public CinemachineImpulseSource impulseSource_speeding, impulseSource_out;

    void Start()
    {
        pose = transform.position;
        initialpos = 390f;
        finalpos = 427f;
        skincolor = 4;
        rotationSpeed = 20f;
        levl = 0;
        slider.value = 1;
        trail = gameObject.GetComponent<TrailRenderer>();
        sprite = sprite_boundary.GetComponent<SpriteRenderer>();
        sprite.DOColor(boundary_color, 1f).SetLoops(-1, LoopType.Yoyo);
        impulseSource_speeding = GetComponent<CinemachineImpulseSource>();
        mainCanvas.SetActive(true);
        // Save the original position of the camera

        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        postProcessVolume = volumePost.GetComponent<PostProcessVolume>();

        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out vignette);
            virgenetteInitial = vignette.intensity.value;
            postProcessVolume.profile.TryGetSettings(out bloom);
            bloomInitial = bloom.intensity.value;
            postProcessVolume.profile.TryGetSettings(out chromatic);
            chromaticInitial = chromatic.intensity.value;
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
        movement = new Vector2(movementx, movementY);
        movement.Normalize();
        foreach(GameObject obstacle in obstacles)
        {
            obstacle.transform.Rotate(new Vector3(0f, 0f, 0.5f));
        }
        Move(movement);
        power();
        if(Input.touchCount > 2)
        {
            SceneManager.LoadScene("level3");
        }
    }

    void Move(Vector2 direction)
    {
        // Apply movement using the Rigidbody2D component
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
        currentDirection = Vector2.Lerp(currentDirection, movement.normalized, rotationSpeed * Time.fixedDeltaTime);
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "obstacle")
        {
            spawn();
            lives--;
            Destroy(heart[lives]);
            FindObjectOfType<AudioMnagaer>().Play("restart");
            camerashakemanager.instance.cameraShake(impulseSource_out);
            Instantiate(particles, transform.position, Quaternion.identity);
        }
        else if (collision.gameObject.tag == "finish1")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            transform.position = new Vector2(75.1f, 153f);
            levl = 1;
            bloom.intensity.value += 10f;
            Invoke("finish", 0.8f);
        }
        else if (collision.gameObject.tag == "finish2")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            transform.position = new Vector2(76.1f, 290f);
            levl = 2;
            Invoke("finish", 0.8f);
        }
        else if (collision.gameObject.tag == "finish3")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            transform.position = new Vector2(76.1f, 347f);
            levl = 3;
            Invoke("finish", 0.8f);
        }
        else if (collision.gameObject.tag == "finish4")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            transform.position = new Vector2(76.1f, 393f);
            levl = 4;
            Invoke("finish", 0.8f);
        }
        else if (collision.gameObject.tag == "finish5")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            SceneManager.LoadScene("levl2");
            
        }
        else if (collision.gameObject.tag == "finish6")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            SceneManager.LoadScene("level3");
            
        }
        else if (collision.gameObject.tag == "finish7")
        {
            FindObjectOfType<AudioMnagaer>().Play("levelcmplt");
            SceneManager.LoadScene("level4");

        }
        else if(collision.gameObject.tag == "cheat_code_4")
        {
            SceneManager.LoadScene("level4");
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
    }
    */
    public void clickPause()
    {
        mainCanvas.SetActive(false);
        FindObjectOfType<AudioMnagaer>().Play("button_click");
        joystickCanvas.SetActive(true);
    }
    public void clickPlay()
    {
        mainCanvas.SetActive(true);
        FindObjectOfType<AudioMnagaer>().Play("button_click");
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
                FindObjectOfType<AudioMnagaer>().Play("power");
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
            rb.velocity = new Vector2(rb.velocity.x * acceleration + Time.deltaTime, rb.velocity.y + acceleration + Time.deltaTime);
            slider.value -= Time.deltaTime * 0.2f;
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
            vignette.intensity.value = Mathf.Min(vignette.intensity.value, 0.55f);
            speedingEfct.SetActive(true);
            camerashakemanager.instance.cameraShake(impulseSource_speeding);          // Increase the vignette intensity
            vignette.intensity.value += 0.05f;
            chromatic.intensity.value = Mathf.Min(chromatic.intensity.value, 0.65f);
            chromatic.intensity.value += 0.05f;
        }
        else if(Input.GetButton("Jump") && vignette != null)
        {
            // Clamp vignette intensity to a maximum value if needed
            vignette.intensity.value = Mathf.Min(vignette.intensity.value, 0.35f);
            speedingEfct.SetActive(true);
            camerashakemanager.instance.cameraShake(impulseSource_speeding);          // Increase the vignette intensity

            chromatic.intensity.value = Mathf.Min(chromatic.intensity.value, 0.65f);
            chromatic.intensity.value += 0.05f;

            // Increase the vignette intensity 
            vignette.intensity.value += 0.05f;
            
        }
        else if (vignette != null)
        {
            // Reset vignette intensity when not touching the screen
            vignette.intensity.value = virgenetteInitial;
            chromatic.intensity.value = chromaticInitial;
            speedingEfct.SetActive(false);
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
    void finish()
    {
        bloom.intensity.value = bloomInitial;
    }
    // Function to start the camera shake
}

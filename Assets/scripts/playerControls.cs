using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    public float carSpeed = 10.0f;
    public GameObject particles;
    public Joystick joystick;
    public float speed;
    private float movementx, movementY;
    

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        movementx = joystick.Horizontal * speed;
        movementY = joystick.Vertical * speed;
        transform.Translate(movementx, movementY, 0);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "obstacle")
        {
            Instantiate(particles, transform.position, Quaternion.identity);
        }
    }
}

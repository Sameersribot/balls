using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpingballs : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 initialpos;
    // Start is called before the first frame update
    void Start()
    {
        initialpos = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            rb.AddForce(new Vector2(0f, 30f));
        }
        else if(collision.gameObject.tag == "obstacle")
        {
            transform.position = initialpos;
        }
    }
}

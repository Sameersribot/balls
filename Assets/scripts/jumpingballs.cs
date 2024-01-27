using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpingballs : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject obsRed, redBall, z;
    private Vector3 initialpos, fixedPos;
    private bool isFixed;
    // Start is called before the first frame update
    void Start()
    {
        initialpos = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();

        isFixed = false;
        InvokeRepeating("ballInstantiate", 2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isFixed)
        {
            gameObject.transform.position = fixedPos;
        }
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "blue")
        {
            fixedPos= collision.gameObject.transform.position;
            isFixed = true;
            obsRed.SetActive(false);
        }
        
    }
    void ballInstantiate()
    {
        GameObject game =  Instantiate(redBall, z.transform.position, Quaternion.identity);
        Destroy(game, 9f);
    }
    
}

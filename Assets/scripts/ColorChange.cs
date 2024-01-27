using System.Collections;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    // Array of colors to choose from
    public Color[] colors;
    private Collider2D collider;
    // Time interval for color change in seconds
    public float colorChangeInterval;

    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        InvokeRepeating("ChangeColorRandomly", colorChangeInterval, 1f);

        collider = gameObject.GetComponent<Collider2D>();
    }
    //js
    // Coroutine to handle the color change
    

    // Function to change the color randomly
    private void ChangeColorRandomly()
    {
        // Randomly choose an index from the colors array

        int randomIndex = Random.Range(0, colors.Length);
        if(randomIndex == 1)
        {
            gameObject.tag = "Untagged";
            collider.isTrigger = true;
        }
        else
        {
            gameObject.tag = "obstacle";
            collider.isTrigger = false;
        }
        // Set the SpriteRenderer's color to the chosen color
        spriteRenderer.color = colors[randomIndex];
    }
}

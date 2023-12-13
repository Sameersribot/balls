using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    private AudioSource audioSource;
    public float scaleAmount = 0.15f; // Scale amount when pressed
    public float pressDuration = 0.01f; // Duration of the pressing effect
    public Button[] buttonsToScale; // Specify the buttons you want to apply the effect to
    public int buttonIndex;

    private bool isScaling = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        if (buttonsToScale == null || buttonsToScale.Length == 0)
        {
            Debug.LogError("Please assign buttons to the 'buttonsToScale' array in the inspector.");
            return;
        }

        // Attach the OnClick method to each button
        foreach (Button button in buttonsToScale)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if we are currently scaling
        if (isScaling)
        {
            // Reduce the scale over time to create a smooth effect
            //foreach (Button button in buttonsToScale)
            //{
            buttonsToScale[buttonIndex].transform.localScale = Vector3.Lerp(buttonsToScale[buttonIndex].transform.localScale, Vector3.one*0.48f, Time.deltaTime * 10f);
            //}
        }
        else
        {
            
        }
    }
    public void clickPlay()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void OnButtonClick(Button button)
    {
        // Start the pressing effect
        audioSource.Play();
        for (int i=0;i<=buttonsToScale.Length;i++)
        {
            if(buttonsToScale[i] = button)
            {
                buttonIndex = i;
                ScaleButton(button);
            }
        }
    }
    private void ScaleButton(Button button)
    {
        // Scale up
        button.transform.localScale = Vector3.one * scaleAmount*0.48f;
        isScaling = true;

        // Reset the scaling after the specified duration
        Invoke("ResetScaling", pressDuration);
    }

    private void ResetScaling()
    {
        isScaling = false;
    }

}

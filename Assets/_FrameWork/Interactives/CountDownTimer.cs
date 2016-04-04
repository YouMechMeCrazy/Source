using UnityEngine;
using System.Collections;

public class CountDownTimer : MonoBehaviour {

    public TextMesh display;
    public MeshRenderer back;

    
    public float timerDuration = 5f;
    [SerializeField]
    float hurriedTime = 1f;

    private float currentTime = 0f;
    private float startTime = 0f;
    private bool enabled = true;

    public float scaleSpeed = 0.1f;
    float scale = 0f;

    [SerializeField]
    Color initialColor;
    [SerializeField]
    Color hurriedColor;
    

    public void Enable() 
    {
        display.gameObject.SetActive(true);
        back.gameObject.SetActive(true);
        startTime = Time.time;
        currentTime = timerDuration;
        transform.localScale = new Vector3(scale, scale, scale);
        enabled = true;
        display.color = initialColor;
    }

    public void Disable() 
    {
        enabled = false;
        display.gameObject.SetActive(false);
        back.gameObject.SetActive(false);
    }

    void Update() 
    {
      
        if (enabled)
        {
            CountDown();
        }
    }

    void CountDown() 
    {
        display.text = currentTime.ToString("F1");
        currentTime -= Time.deltaTime;

        if (scale <= 1f && currentTime > 0f)
        {
            scale += scaleSpeed;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        if (currentTime <= hurriedTime)
        {
            display.color = hurriedColor;
        }

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            scale -= scaleSpeed;
            transform.localScale = new Vector3(scale, scale, scale);
            if (scale <= 0f)
            {
                Disable();
            }
        }
    }
}

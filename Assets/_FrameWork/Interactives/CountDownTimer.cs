using UnityEngine;
using System.Collections;

public class CountDownTimer : MonoBehaviour {

    public TextMesh display;
    public MeshRenderer back;

    
    public float timerDuration = 5f;

    private float currentTime = 0f;
    private float startTime = 0f;
    private bool enabled = true;


    public void Enable() 
    {
        display.gameObject.SetActive(true);
        back.gameObject.SetActive(true);
        startTime = Time.time;
        currentTime = timerDuration;
        enabled = true;
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

        if (currentTime <= 0f)
        {
            Disable();
        }
    }
}

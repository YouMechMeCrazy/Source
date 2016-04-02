using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {


    public Button start;
    public Button credits;

    public Color unSelected;
    public Color cSelected;

    public Animation titleScreen;

    public GameObject creditPanel;

    bool inCredits = false;

    int selected = 0;

    public GameObject fadeScreen;

    bool isFading = false;
    float fadingStartTime;
    [SerializeField]
    float fadingTime;

   

    void Start () 
    {
        SoundController.Instance.PlayMusic("Title_Screen");
        Time.timeScale = 1f;
        bool worked = titleScreen.Play();
        Debug.Log(worked);
	}
	
	// Update is called once per frame
    void Update()
    {

        if (isFading)
        {
            FadeToBack();
            return;
        }

        if (Input.GetAxis("Vertical") < -0.5f || Input.GetAxis("Vertical2") < -0.5f)
        {
            selected++;
            if (selected > 1)
            {
                selected = 1;
            }
        }
        if (Input.GetAxis("Vertical") > 0.5f || Input.GetAxis("Vertical2") > 0.5f)
        {
            selected--;
            if (selected < 0)
            {
                selected = 0;
            }
        }

        if (Input.GetButtonDown("Fire1") )
        {
            if (selected == 0)
            {
                LoadLevel();
            }
            else if(!inCredits)
            {
                Credits();
            }
        }

        if (Input.GetButtonDown("Fire2") && inCredits)
        {
            inCredits = false;
            DisplayCredits();
            titleScreen.Play();
            credits.GetComponent<Animator>().SetBool("isPressed", false);
            start.GetComponent<Animator>().SetBool("isPressed", false);

         

            foreach (AnimationState state in titleScreen)
            {
                state.time = 0f;
                state.speed = 1F;
            }
        }


        if (selected == 0)
        {

            start.GetComponent<Animator>().SetBool("isSelected", true);
            credits.GetComponent<Animator>().SetBool("isSelected", false);
        }
        else 
        {
            start.GetComponent<Animator>().SetBool("isSelected", false);
            credits.GetComponent<Animator>().SetBool("isSelected", true);
        }


       

    }

    public void Credits() 
    {
        inCredits = true;
        Invoke("DisplayCredits", 1f);
        titleScreen.Play();

        credits.GetComponent<Animator>().SetBool("isPressed", true);
        start.GetComponent<Animator>().SetBool("isPressed", true);

        foreach (AnimationState state in titleScreen)
        {
            state.time = state.length;
            state.speed = -1F;
        }
    }

    void DisplayCredits() 
    {
        if (inCredits)
            creditPanel.GetComponent<Text>().color = unSelected;
        else
            creditPanel.GetComponent<Text>().color = new Color(0f,0f,0f,0f);
    }


    public void LoadLevel() 
    {
        StartCoroutine(DelaySceneLoad());
        isFading = true;
        fadingStartTime = Time.time;
    }


    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(fadingTime);
        isFading = false;
        SceneManager.LoadSceneAsync("World_Selection");
    }


    void FadeToBack()
    {
        SoundController.Instance.Volume(-0.005f);
        fadeScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, (Time.time - fadingStartTime) / fadingTime);
       
    }

}

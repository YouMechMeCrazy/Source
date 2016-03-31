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

    void Start () 
    {
        SoundController.Instance.PlayMusic("RoboParty");
        Time.timeScale = 1f;
        bool worked = titleScreen.Play();
        Debug.Log(worked);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Horizontal2") > 0f)
        {
            selected++;
            if (selected > 1)
            {
                selected = 1;
            }
        }
        if (Input.GetAxis("Horizontal") < 0f || Input.GetAxis("Horizontal2") < 0f)
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
            foreach (AnimationState state in titleScreen)
            {
                state.time = 0f;
                state.speed = 1F;
            }
        }


        if (selected == 0)
        {
            start.transform.FindChild("Text").GetComponent<Text>().color = cSelected;
            credits.transform.FindChild("Text").GetComponent<Text>().color = unSelected;
        }
        else 
        {
            start.transform.FindChild("Text").GetComponent<Text>().color = unSelected;
            credits.transform.FindChild("Text").GetComponent<Text>().color = cSelected;
        }

    }

    public void Credits() 
    {
        inCredits = true;
        Invoke("DisplayCredits", 1f);
        titleScreen.Play();
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
        SceneManager.LoadScene("World_Selection");
    }
}

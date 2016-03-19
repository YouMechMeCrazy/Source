using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

    GameObject canvas;

    public int selected = 0;
    public bool isPaused = false;

    public Button unpause;
    public Button mainMenu;
    
    


	// Use this for initialization
	void Start () {
        canvas = transform.FindChild("PauseScreenCanvas").gameObject;
        canvas.SetActive(false);
        

	}
	
	// Update is called once per frame
	void Update () {

        if (isPaused)
        {
            if (Input.GetAxis("Vertical") > 0.1 || Input.GetAxis("Vertical2") > 0.1)
            {
                SetSelected(-1);
            }
            if (Input.GetAxis("Vertical") < -0.1 || Input.GetAxis("Vertical2") < -0.1)
            {
                SetSelected(1);
            }
            if (Input.GetButtonDown("Submit2") || Input.GetButtonDown("Submit"))
            {
                
                if (selected == 0)
                {
                    Resume();
                }
                else
                {
                    MainMenu();
                }
            }
        }

	}
    void SetSelected(int increment) 
    {
        selected += increment;
        if (selected < 0)
        {
            selected = 0;
        }
        if (selected > 1)
        {
            selected = 1;
        }
        if (selected == 0)
        {
            unpause.GetComponent<Image>().color = new Color(1f,0f,0f,1f);
            mainMenu.GetComponent<Image>().color = new Color(0f, 1f, 1f, 1f);
        }
        if (selected == 1)
        {
            mainMenu.GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            unpause.GetComponent<Image>().color = new Color(0f, 1f, 1f, 1f);
        }
        
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Resume() {
        GameController.Instance.Pause();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Show() {
        canvas.SetActive(true);
    }
    public void Hide() {
        canvas.SetActive(false);
    }
}

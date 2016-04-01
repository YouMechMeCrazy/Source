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

    public float normalScale;
    public float selectedScale;
    public Color unSelected;
    public Color CSelected;


	// Use this for initialization
	void Start () {
        canvas = transform.FindChild("PauseScreenCanvas").gameObject;
        canvas.SetActive(false);

        SetSelected(0);
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
            if (Input.GetButtonDown("Fire1"))
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
            if (Input.GetButtonDown("Fire2"))
            {
                Resume();
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
            unpause.GetComponent<Image>().color = CSelected;
            unpause.transform.localScale = new Vector3(selectedScale, selectedScale, selectedScale);

            mainMenu.GetComponent<Image>().color = unSelected;
            mainMenu.transform.localScale = new Vector3(normalScale, normalScale, normalScale);

        }
        if (selected == 1)
        {
            mainMenu.GetComponent<Image>().color = CSelected;
            mainMenu.transform.localScale = new Vector3(selectedScale, selectedScale, selectedScale);

            unpause.GetComponent<Image>().color = unSelected;
            unpause.transform.localScale = new Vector3(normalScale, normalScale, normalScale);
        }
        
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Resume() 
    {
        GameController.Instance.Pause();
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Show() 
    {
        canvas.SetActive(true);
    }
    public void Hide() 
    {
        canvas.SetActive(false);
    }
}

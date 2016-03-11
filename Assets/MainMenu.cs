using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    GameObject[] Submenus;
    int activeMenu = 0;
    EventSystem myEventSystem;
    [SerializeField]
    GameObject[] StartingButtons;

   bool Abutton;
    // Use this for initialization
    void Start () {
        StartingButtons[0].GetComponent<Selectable>().Select();
        myEventSystem = GetComponent<EventSystem>();
        Submenus = new GameObject[3];
        Submenus[0] = transform.FindChild("MainMenu").gameObject;
        Submenus[1] = transform.FindChild("LevelSelect").gameObject;
        Submenus[2] = transform.FindChild("Credits").gameObject;


	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < 3; i++) {
            if (activeMenu == i) { Submenus[i].SetActive(true); }
            else { Submenus[i].SetActive(false); }
        }

        
        if (Input.GetMouseButtonDown(1) || Input.GetAxis("Fire2") > 0) {
            SwitchToMenu(0);
        }



        if(activeMenu == 2)
        {
            if (Input.GetAxis("Fire1") == 0) { Abutton = true; }
            if (Input.GetAxis("Fire1") > 0 && Abutton) { SwitchToMenu(0); }
            
        }
        else { Abutton = false; }



	}

    public void SwitchToMenu(int menu) {
        activeMenu = menu;
        Submenus[menu].SetActive(true);
        myEventSystem.SetSelectedGameObject(StartingButtons[menu]);
        StartingButtons[menu].GetComponent<Selectable>().Select();
    }

    public void LoadLevel(string levelToLoad) {
        SceneManager.LoadScene(levelToLoad);
        
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = transform.FindChild("PauseScreenCanvas").gameObject;
        canvas.SetActive(false);
        

	}
	
	// Update is called once per frame
	void Update () {
	
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

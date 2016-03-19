using UnityEngine;
using System.Collections;

public class Ui_Rotate : MonoBehaviour {

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    bool clockwise = true;

	// Use this for initialization
	void Start () 
    {
        if (clockwise)
            speed *= -1f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(0f,0f,speed);
	}
}

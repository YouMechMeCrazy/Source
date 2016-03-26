using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {


    GameObject cameraTarget;

    void Awake()
    {
        cameraTarget = Camera.main.gameObject;
    }
 
	void Update ()
    {
        transform.LookAt(cameraTarget.transform, Vector3.up);

    }
}

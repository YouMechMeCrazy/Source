using UnityEngine;
using System.Collections;

public class Remove_After_Time : MonoBehaviour {

    [SerializeField]
    float time = 2f;
	// Use this for initialization
	void Start () 
    {
        Destroy(gameObject, time);
	}
	
	
}

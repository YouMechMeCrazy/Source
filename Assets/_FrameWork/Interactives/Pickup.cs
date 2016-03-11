using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    [SerializeField]
    float gravity;
    float fallspeed;
    [SerializeField]
    float maxFallSpeed;

    bool isHold;
    Rigidbody rb;
    float maxVelocity = 5f;


    // Use this for initialization
    void Start () 
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f);
	}



    public virtual void OnPickedUp() {
        
        isHold = true;
        
    }

    public virtual void OnPutDown() {
        
        isHold = false;
    }

    public virtual float GetSize() {
        return 0.5f;
    }
}

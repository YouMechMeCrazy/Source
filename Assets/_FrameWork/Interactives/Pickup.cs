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
        if (gameObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        
    }

    public virtual void OnPutDown() 
    {
        SoundController.Instance.PlayFX("Box_Drop_Metal", transform.position);
        isHold = false;
        if (gameObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    public virtual float GetSize() {
        return 0.5f;
    }
}

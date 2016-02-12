using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    [SerializeField]
    float gravity;
    float fallspeed;
    [SerializeField]
    float maxFallSpeed;

    bool isHold;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (!isHold) { Gravity(); }
	}

   protected virtual void Gravity() {
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position, new Vector3(0.3f, 0.3f, 0.3f), Vector3.down, Quaternion.identity, fallspeed * Time.deltaTime + 0.5f);
        bool ok = false;
        if (hits.Length > 0)
        {
            
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform != transform) { ok = true; }
            }

            if (ok)
            {
                fallspeed = 0f;
                float lowesty = 1000f;
                for (var i = 0; i < hits.Length; i++)
                {
                    if (hits[i].distance < lowesty && hits[i].transform!= transform) { lowesty = hits[i].distance; }
                }
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f - lowesty, transform.position.z);
            }

        }
        if(!ok) {
            fallspeed = Mathf.Min(fallspeed + gravity * Time.deltaTime, maxFallSpeed);
        }

        transform.Translate(new Vector3(0f, -fallspeed * Time.deltaTime, 0f));
    }

    public virtual void OnPickedUp() {
        GetComponent<Collider>().enabled = false;
        isHold = true;
        fallspeed = 0f;
    }

    public virtual void OnPutDown() {
        GetComponent<Collider>().enabled = true;
        isHold = false;
    }

    public virtual float GetSize() {
        return 0.5f;
    }
}

using UnityEngine;
using System.Collections;

public class Laser : InputObject {


    [SerializeField]
    float length;
    Transform Beam;

    [SerializeField]
    bool defaultActive;

	// Use this for initialization
	void Start () {
        Beam = transform.FindChild("LaserBeam");
        if (defaultActive) { state = input._Off; } else { state = input._On; }
	}
	
	// Update is called once per frame
	void Update () {

        if (state == input._On) {
            Beam.GetComponent<Renderer>().enabled = true;
            Beam.transform.localPosition = new Vector3(length / 2, 0f, 0f);
            Beam.transform.localScale = new Vector3(0.1f,length/2, 0.1f);

        }
        if (state == input._Off) {
            Beam.GetComponent<Renderer>().enabled = false;
        }




	}
}

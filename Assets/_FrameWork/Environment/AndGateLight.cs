using UnityEngine;
using System.Collections;

public class AndGateLight : InputObject {

    [SerializeField]
    float resetTimer;

    float timer;


	// Use this for initialization
	void Start () {
	
	}

    protected override void TurnOn()
    {
        base.TurnOn();
        timer = resetTimer;
        GetComponent<Renderer>().material.color = new Color(0, 1, 0);
    }
    protected override void TurnOff()
    {
        base.TurnOff();
        GetComponent<Renderer>().material.color = new Color(1, 0, 0);
    }

    public bool IsOn() {
        return (state == input._On);
    }


    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime;
        if (timer < 0) { Input(input._Off);}
	}


}

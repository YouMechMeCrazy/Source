using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Vector3 input;
    Vector3 arminput;
    float facingAngle;
    float armFacingAngle;
    Vector3 facingVector;
    Vector3 armFacingVector;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotateSpeed; //in degrees
    [SerializeField]
    float armRotateSpeed; //in degrees
    Transform legs; //the correct answer
    Transform arms; //who needs arms with legs like these
    Vector3 center;
    [SerializeField]
    float gravity;
    float fallspeed;
    [SerializeField]
    float maxFallSpeed;
    [SerializeField]
    float reach = 1;

    Pickup holding = null;
    Vector3 holdingAngle;
    float holdingrotate = 0;

    [SerializeField]
    bool player2;

	// Use this for initialization
	void Start () {
        facingVector = new Vector3(1f, 0f, 0f);
        legs = transform.FindChild("Legs");
        arms = transform.FindChild("Arms");
        fallspeed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        string inputbonus = "";
        if (player2) { inputbonus = "2"; }

        center = transform.position + new Vector3(0f, 1f, 0f);

        facingVector = Quaternion.AngleAxis(facingAngle, Vector3.up)*Vector3.right;
        armFacingVector = Quaternion.AngleAxis(armFacingAngle, Vector3.up) * Vector3.right;

        input = new Vector3(Input.GetAxis("Horizontal"+inputbonus), 0f, Input.GetAxis("Vertical" + inputbonus));
        arminput = new Vector3(Input.GetAxis("HorizontalArms" + inputbonus), 0f, Input.GetAxis("VerticalArms" + inputbonus));

        if (input != Vector3.zero) {
            LegRotate();
        }
        if (arminput != Vector3.zero)
        {
            ArmRotate();
        }

        Movement();

        Gravity();

        PickUp();

        legs.rotation = Quaternion.LookRotation(facingVector.normalized);
        arms.rotation = Quaternion.LookRotation(armFacingVector.normalized);

    }


    void PickUp() {

        if (holding!= null) {
             holding.transform.position = center + armFacingVector * (reach+holding.GetSize());
            
            holdingrotate = Vector2.Angle(new Vector2(armFacingVector.x,armFacingVector.z), new Vector2(holdingAngle.x,holdingAngle.z));
            //Debug.Log(holdingrotate);
            Vector3 cross = Vector3.Cross(new Vector2(armFacingVector.x, armFacingVector.z), new Vector2(holdingAngle.x, holdingAngle.z));
            if (cross.z < 0) { holdingrotate = 360 - holdingrotate; }
            
            holding.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(holdingrotate, Vector3.up) * Vector3.right);

        }


        if (Input.GetAxis("PickUp") > 0)
        {
            if (holding == null)
            {
                RaycastHit[] hits = Physics.BoxCastAll(center, new Vector3(0.1f, 0.1f, 0.1f), armFacingVector, Quaternion.identity, reach);
                for (int i = 0; i < hits.Length; i++)
                {
                    Pickup temp = hits[i].transform.GetComponent<Pickup>();
                    if (temp != null)
                    {
                        temp.OnPickedUp();
                        holding = temp;
                        holding.transform.position = center + armFacingVector * (reach + holding.GetSize());
                        holdingAngle = (holding.transform.position - transform.position).normalized;
                        holdingrotate = 0;
                    }
                }



            }
        }
        else {
            if (holding != null) { holding.OnPutDown(); holding = null; }
        }


    }

    void OnTriggerStay(Collider other) {
        if (other.GetComponent<Button>()&&Input.GetButtonDown("PickUp")){
            other.GetComponent<Button>().Hit();
        }
    }


    void Movement() {
        float speedMult = GetSpeedMult();
        float speed = moveSpeed * speedMult * Time.deltaTime;

        Vector3 dirvector = facingVector;
        bool ok = true;
        //bool check = false;
        //if (holding != null) { check = Physics.CheckSphere(holding.transform.position+(facingVector* speed), holding.GetSize()); }
        //if (!check)
        //{
            //if (Physics.CheckSphere(center + (facingVector * speed), 0.5f))
            if(WallCheck(facingVector*speed))
            {
                ok = false;
                for (var i = 0; i < 60; i += 5)
                {
                    dirvector = Quaternion.AngleAxis(facingAngle + i, Vector3.up) * Vector3.right;
                    if (!WallCheck(dirvector* speed)) { ok = true; break; }
                    dirvector = Quaternion.AngleAxis(facingAngle - i, Vector3.up) * Vector3.right;
                    if (!WallCheck(dirvector * speed)) { ok = true; break; }
                }

            }
            if (ok)
            {
                transform.Translate(dirvector * speed);
            }
        //}
    }

    void Gravity()
    {

        RaycastHit[] hits;
        hits = Physics.BoxCastAll(center, new Vector3(0.3f, 0.3f, 0.3f), Vector3.down, Quaternion.identity, fallspeed * Time.deltaTime+0.5f,1,QueryTriggerInteraction.Ignore);
       
        if (hits.Length > 0)
        {
            fallspeed = 0f;
            float lowesty = 1000f;
            for (var i = 0; i < hits.Length; i++) {
                if (hits[i].distance < lowesty) { lowesty = hits[i].distance; }
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f - lowesty, transform.position.z);

        }
        else {
            fallspeed = Mathf.Min(fallspeed + gravity * Time.deltaTime, maxFallSpeed);
        }

        transform.Translate(new Vector3(0f, -fallspeed*Time.deltaTime,0f));
    }


    float GetSpeedMult() {
        Vector2 v1 = new Vector2(facingVector.x, facingVector.z).normalized;
        Vector2 v2 = new Vector2(input.x, input.z).normalized;

        if (v2 != Vector2.zero)
        {
            float f = Vector2.Dot(v1, v2);
            f = Mathf.Clamp(f, -1, 1);//just in case.
            f = f / 2 + 0.5f; //make it a number between 0 and 1
           // Debug.Log(f);
            return f;
        }
        else {
            return 0f;
        }


    }

    void LegRotate() {
        
        float inputangle = Vector2.Angle(new Vector2(input.x, input.z), Vector2.right)+0.01f;
        Vector3 cross = Vector3.Cross(new Vector2(input.x, input.z), Vector2.right);
        if (cross.z < 0) { inputangle = 360 - inputangle; }
        
        float f = Mathf.Sign(Mathf.DeltaAngle(facingAngle, inputangle)) * rotateSpeed * Time.deltaTime;

        if (Mathf.Abs(f) < Mathf.Abs(Mathf.DeltaAngle(facingAngle, inputangle))) { facingAngle += f; }
        else { facingAngle = inputangle; }        
        //Debug.Log(facingangle.ToString() + " " + inputangle.ToString());
    }

    void ArmRotate()
    {

        float inputangle = Vector2.Angle(new Vector2(arminput.x, arminput.z), Vector2.right) + 0.01f;
        Vector3 cross = Vector3.Cross(new Vector2(arminput.x, arminput.z), Vector2.right);
        if (cross.z < 0) { inputangle = 360 - inputangle; }

        float f = Mathf.Sign(Mathf.DeltaAngle(armFacingAngle, inputangle)) * armRotateSpeed * Time.deltaTime;

        float oldFacingAngle = armFacingAngle;

        if (Mathf.Abs(f) < Mathf.Abs(Mathf.DeltaAngle(armFacingAngle, inputangle))) { armFacingAngle += f; }
        else { armFacingAngle = inputangle; }

        if (holding != null)
        {
            armFacingVector = Quaternion.AngleAxis(armFacingAngle, Vector3.up) * Vector3.right;
            if (Physics.CheckSphere(center+armFacingVector*(reach+holding.GetSize()), holding.GetSize(),1, QueryTriggerInteraction.Ignore)) { armFacingAngle = oldFacingAngle; }
        }

        //Debug.Log(facingangle.ToString() + " " + inputangle.ToString());
    }

    bool WallCheck(Vector3 offset) {
        
        if (holding != null) { if( Physics.CheckSphere(holding.transform.position + offset, holding.GetSize(),1,QueryTriggerInteraction.Ignore)){ return true; } }
        if (Physics.CheckSphere(center + offset, 0.5f, 1, QueryTriggerInteraction.Ignore)){ return true; }
        return false;
        
    }


}

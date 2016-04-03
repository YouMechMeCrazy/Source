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
    float reach = 1;

    [SerializeField]
    float deathAnimDuration = 3f;
    [SerializeField]
    float reviveAnimDuration = 3f;

    Animator animTop;
    Animator animBot;

    Pickup holding = null;
    Vector3 holdingAngle;
    float holdingrotate = 0;

    [SerializeField]
    bool player2;

    [SerializeField]
    float playerHoldingHigth = 2.4f;

    [SerializeField]
    Transform armsStartingLocation;
    [SerializeField]
    float walkSpeedAnimation = 2f;

    private bool hasControl = true;
    private float pickUpStartTime = 0f;
    private bool isPressing = false;

    private string inputbonus = "";
    private bool isBroken = true;

    Rigidbody rb;

    void Awake()
    {
        animTop = transform.FindChild("Arms").transform.FindChild("Top").GetComponent<Animator>();
        animBot = transform.FindChild("Legs").transform.FindChild("Bot").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    // Use this for initialization
    void Start() 
    {
        facingVector = new Vector3(1f, 0f, 0f);
        legs = transform.FindChild("Legs");
        arms = transform.FindChild("Arms");
        //if broken
        GetComponent<CapsuleCollider>().center = new Vector3(0f, 1.5f, 0f);
        GetComponent<CapsuleCollider>().height = 2.5f;

        if (player2)
        {
            facingAngle = 45f;
        }    
        else
        {
            facingAngle = 135f;
        }
        if (player2) { inputbonus = "2"; }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonUp("Submit" + inputbonus) && GameController.Instance.TimeAtPause() < Time.time - 0.5f)
        {
            GameController.Instance.Pause();
        }
        if (isBroken)
        {
            BrokenState();
        }

        if (!hasControl)
        {
            return;
        }
   
       

        center = transform.position + new Vector3(0f, 1f, 0f);

        facingVector = Quaternion.AngleAxis(facingAngle, Vector3.up) * Vector3.right;
        armFacingVector = Quaternion.AngleAxis(armFacingAngle, Vector3.up) * Vector3.right;


        input = new Vector3(Input.GetAxis("Horizontal" + inputbonus), 0f, Input.GetAxis("Vertical" + inputbonus));
        arminput = new Vector3(Input.GetAxis("HorizontalArms" + inputbonus), 0f, Input.GetAxis("VerticalArms" + inputbonus));

        if (input != Vector3.zero) 
        {
            LegRotate();
        }
        if (arminput != Vector3.zero)
        {
            ArmRotate();
        }

        Movement();

        
        PickUp(inputbonus);

        legs.rotation = Quaternion.LookRotation(facingVector.normalized);
        arms.rotation = Quaternion.LookRotation(armFacingVector.normalized);

       

    }




    void PickUp(string inputbonus) 
    {

        if (holding != null) 
        {
            holding.transform.position = center + armFacingVector * (reach + holding.GetSize()) + new Vector3(0f, Mathf.Clamp(playerHoldingHigth * ((Time.time - pickUpStartTime) * 10f), 0f, playerHoldingHigth), 0f);

            holdingrotate = Vector2.Angle(new Vector2(armFacingVector.x, armFacingVector.z), new Vector2(holdingAngle.x, holdingAngle.z));

            Vector3 cross = Vector3.Cross(new Vector2(armFacingVector.x, armFacingVector.z), new Vector2(holdingAngle.x, holdingAngle.z));
            if (cross.z < 0) 
            { 
                holdingrotate = 360 - holdingrotate; 
            }

            holding.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(holdingrotate, Vector3.up) * Vector3.right);

        }

        if (Input.GetAxis("PickUp" + inputbonus) < 0 || Input.GetAxis("PickUp" + inputbonus) > 0)
        {
            if (holding == null)
            {
                RaycastHit[] hits = Physics.BoxCastAll(center, new Vector3(0.1f, 0.1f, 0.1f), armFacingVector, Quaternion.identity, reach);
                for (int i = 0; i < hits.Length; i++)
                {
                    Pickup temp = hits[i].transform.GetComponent<Pickup>();
                    if (temp != null)
                    {
                        SoundController.Instance.PlayFX("Mech_PickUp", transform.position);

                        temp.OnPickedUp();
                        holding = temp;
                        holding.transform.position = (center + armFacingVector * (reach + holding.GetSize()));
                        holdingAngle = (holding.transform.position - transform.position).normalized;
                        holdingrotate = 0;
                        pickUpStartTime = Time.time + 0.25f;
                        animTop.SetBool("pickingUp", true);
                        GetComponent<Weight>().AddWeight(holding.gameObject.GetComponent<Weight>().GetWeight());
                    }
                    if (hits[i].transform.GetComponent<InteractiveButton>() != null && !isPressing)
                    {
                        SoundController.Instance.PlayFX("Mech_Press_Button", transform.position);

                        animTop.SetTrigger("pressingButton");
                        hits[i].transform.GetComponent<InteractiveButton>().Hit();
                        isPressing = true;
                    }

                }



            }
        }
        else
        {
            if (holding != null) 
            {
                GetComponent<Weight>().RemoveWeight(holding.gameObject.GetComponent<Weight>().GetWeight());
                holding.OnPutDown(); holding = null;
                animTop.SetBool("pickingUp", false);

                SoundController.Instance.PlayFX("Mech_Release", transform.position);
            }
            isPressing = false;
        }


    }

    void OnTriggerStay(Collider other) 
    {
        if (other.GetComponent<InteractiveButton>() && Input.GetButtonDown("PickUp")) 
        {
            other.GetComponent<InteractiveButton>().Hit();
        }
    }


    //audio componenet of movement.
    float walkTimer = 0f;
    float walkClipDuration = 0f;

    void Movement() 
    {

        float xAxis = Mathf.Abs(Input.GetAxis("Horizontal" + inputbonus));
        if (xAxis < 0.1f)
        {
            xAxis = 0f;
        }

        float yAxis = Mathf.Abs(Input.GetAxis("Vertical" + inputbonus));
        if (yAxis < 0.1f)
        {
            yAxis = 0f;
        }

        float[] move = new float[] { xAxis, yAxis };

       

        Vector3 dirvector = new Vector3(facingVector.x, 1f, facingVector.z) * moveSpeed * Mathf.Max(move) * Mathf.Clamp(Time.deltaTime, 0f, 0.02f);


        rb.velocity = new Vector3(dirvector.x, rb.velocity.y, dirvector.z);

        if (Mathf.Max(move) != 0 && walkTimer < Time.time )
        {
            if (!player2)
            {
                walkClipDuration = SoundController.Instance.PlayFX("Player1_Walk", transform.position);

            }
            else
            {
                walkClipDuration = SoundController.Instance.PlayFX("Player2_Walk", transform.position);
            }
            walkTimer = Time.time + walkClipDuration;
                
        }


        animBot.SetFloat("move", Mathf.Max(move) * walkSpeedAnimation);
        if(!isBroken)
            animTop.SetFloat("move", Mathf.Max(move) * walkSpeedAnimation);
      
    }





    void LegRotate() 
    {

        float inputangle = Vector2.Angle(new Vector2(input.x, input.z), Vector2.right) + 0.01f;
        Vector3 cross = Vector3.Cross(new Vector2(input.x, input.z), Vector2.right);
        if (cross.z < 0) { inputangle = 360 - inputangle; }

        float f = Mathf.Sign(Mathf.DeltaAngle(facingAngle, inputangle)) * rotateSpeed * Time.deltaTime;

        if (Mathf.Abs(f) < Mathf.Abs(Mathf.DeltaAngle(facingAngle, inputangle))) { facingAngle += f; }
        else { facingAngle = inputangle; }

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
            if (Physics.CheckSphere(center + armFacingVector * (reach + holding.GetSize()), holding.GetSize(), 1, QueryTriggerInteraction.Ignore)) { armFacingAngle = oldFacingAngle; }
        }


    }


    public void Death()
    {
        animBot.SetTrigger("shock");
        animTop.SetTrigger("shock");
        animBot.SetFloat("move", 0f);
        animTop.SetFloat("move", 0f);

        SoundController.Instance.PlayFX("Mech_Laser_Death", transform.position);

        GetComponent<CapsuleCollider>().isTrigger = true;

        hasControl = false;
        rb.velocity = new Vector3(0f,0f,0f);
        rb.isKinematic = true;
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        
        yield return new WaitForSeconds(deathAnimDuration);
        GameController.Instance.TryRespawnPlayer(player2, deathAnimDuration);
        
    }

    public void RespawnPlayer()
    {
        //play anim
        GetComponent<CapsuleCollider>().isTrigger = false;
        SoundController.Instance.PlayFX("Mech_Respawn", transform.position);
        StartCoroutine(RespawnAnimation());
    }

    IEnumerator RespawnAnimation() 
    {
        yield return new WaitForSeconds(reviveAnimDuration);
        rb.isKinematic = false;
        hasControl = true;
    }

    public bool HasControl() 
    {
        return hasControl;
    }

    public bool IsPlayerTwo()
    {
        return player2;
    }

    public void SetPlayerControl(bool playerHasControl) 
    {
        hasControl = playerHasControl;
    }

    public void ZeroVelocity() 
    {
        rb.velocity = new Vector3(0f,0f,0f);
    }

    void BrokenState() 
    {
        arms.position = armsStartingLocation.position;



        if (Vector3.Distance(arms.position, legs.position) < 1.5f)
        {
            isBroken = false;
            arms.localPosition = new Vector3(0f, 0.84f, 0f);
            GetComponent<CapsuleCollider>().height = 3.5f;

            GetComponent<CapsuleCollider>().center = new Vector3(0f, 2.17f, 0f);
            
        }

    }

    public void EndOfLevel() 
    {
        animBot.SetFloat("move", 0f);
        animTop.SetFloat("move", 0f);
        //play winning animation
    }


}

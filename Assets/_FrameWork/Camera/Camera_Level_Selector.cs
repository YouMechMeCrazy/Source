using UnityEngine;
using System.Collections;

public class Camera_Level_Selector : MonoBehaviour {

    [SerializeField]
    LayerMask clickable;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    [SerializeField]
    float sensitivityX = 15F;
    [SerializeField]
    float sensitivityY = 15F;


    float minimumX = -360F;
    float maximumX = 360F;

    [SerializeField]
    float minimumY = -60F;
    [SerializeField]
    float maximumY = 60F;

    float rotationY = 0F;

    [SerializeField]
    Vector3 baseCameraPosition;

    bool isMoving = false;
    Transform target;
    Vector3 startPOS;

    public float speedRatio = 2f;
    float speed = 1f;
    private float startTime;
    private float journeyLength;

    private Quaternion endRotation;

	// Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            ViewInput();
            if (Input.GetAxis("Fire1") != 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f, 0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f, clickable))
                {
                    if (hit.collider.gameObject.name != transform.parent.name)
                    {
                        target = hit.collider.transform;
                        transform.SetParent(hit.collider.transform);
                        startPOS = transform.localPosition;

                        startTime = Time.time;
                        journeyLength = Vector3.Distance(startPOS, baseCameraPosition);

                        speed = journeyLength / speedRatio;

                        isMoving = true;
                       
                    }
                    
                }
            }

          
       
            
        }
        else 
        {
            Move();
        }

    }

    void Move() 
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.localPosition = Vector3.Lerp(startPOS, baseCameraPosition, fracJourney);
        transform.LookAt(transform.parent.position);

        if (Vector3.Distance(transform.localPosition, baseCameraPosition) < 0.5f)
        {
            rotationY = -transform.localEulerAngles.x;
            isMoving = false;
        }
    }

    void ViewInput() 
    {
        
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX;

            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}



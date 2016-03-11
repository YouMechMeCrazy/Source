using UnityEngine;
using System.Collections;

public class Speach_Bubble : MonoBehaviour
{

    public GameObject cameraTarget;
    [SerializeField][Tooltip("Message to be displayed. The public function ChangeMessage() cant be used to change the content via script.")]
    string textbubble;
    [SerializeField][Tooltip("Makes the bubble appear straight instead of sligthly sweked if the camera is not facing the bubble directly.")]
    bool lockYAxis = false;

    delegate void UpdateDelegate();
    UpdateDelegate uDelagate;

    private float scale = 1f;
    [SerializeField][Tooltip("Base scale for the bubble onces poped Up. SHOULD BE SAME AS defaultScaleUp.")]
    float startingScale = 1f;
    [SerializeField][Tooltip("Increasing or deceasing this value will affect how fast the bubble Pops up or down.")]
    float scaleSpeedUp = 2f;
    [SerializeField][Tooltip("Base value the bubble reaches once finish popping up. SHOULD BE SAME AS startingScale.")]
    float defaultScaleUp = 1f;
    [SerializeField][Tooltip("This value represent the max scale the bubble will reach before scaling back down to the default value. Used to give a cheapo animation.")]
    float overShootScaleUp = 1.2f;



    private float startPopTime;
    private float xReachedTime;
    private float yReachedTime;



    private bool hasOverReachedX = false;
    private bool hasOverReachedY = false;

    

    void Start() 
    {
        uDelagate += LookAt;
        string temp = "";
        int newLineSize = 24;
        int numberOfLines = 1;
        for (int i = 0; i < textbubble.Length; i++)
        {
            temp += textbubble[i];
            if (i >= newLineSize * numberOfLines && textbubble[i] == ' ')
            { 
                temp += '\n';
                numberOfLines++;
            }
            
           
        }
        transform.FindChild("Background").localScale = new Vector3(transform.FindChild("Background").localScale.x, transform.FindChild("Background").localScale.y + numberOfLines, transform.FindChild("Background").localScale.z);
        transform.FindChild("Text").GetComponent<TextMesh>().text = temp;
       
    }

	// Update is called once per frame
	void Update () 
    {
        if (uDelagate != null)
        {
            uDelagate();
        }

       
	}
    void LookAt()
    {
        transform.LookAt(cameraTarget.transform);
        if (lockYAxis)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        }
    }
    void SetScale() 
    {
        scale = startingScale + ((cameraTarget.transform.position.y - 250f) / 200f);
        transform.localScale = new Vector3(scale,scale,scale);
    }

    public void AddPopUp()
    {
        uDelagate += PopUp;
        startPopTime = Time.time;
    }

    void PopUp() 
    {
        float scaleX;
        float scaleY;

        if (!hasOverReachedX)
        {
            scaleX = Mathf.Pow((Time.time - startPopTime) * scaleSpeedUp, 3f);

            if (scaleX >= overShootScaleUp)
            {
                hasOverReachedX = true;
                xReachedTime = Time.time - startPopTime;
            }
        }
        else 
        {
            xReachedTime -= Time.deltaTime;
            scaleX = Mathf.Pow((xReachedTime) * scaleSpeedUp, 3f);
        }
        if (!hasOverReachedY)
        {
            scaleY = Mathf.Pow((Time.time - startPopTime) * scaleSpeedUp, 3f);

            if (scaleY >= overShootScaleUp)
            {
                hasOverReachedY = true;
                yReachedTime = Time.time - startPopTime;
            }
        }
        else 
        {
            yReachedTime -= Time.deltaTime;
            scaleY = Mathf.Pow((xReachedTime) * scaleSpeedUp, 3f);
        }

        if (hasOverReachedX && scaleX <= defaultScaleUp)
        {
            scaleX = defaultScaleUp;
        }
        if (hasOverReachedY && scaleY <= defaultScaleUp)
        {
            scaleY = defaultScaleUp;
        }

        if (hasOverReachedX && hasOverReachedY && scaleX <= defaultScaleUp && scaleY <= defaultScaleUp)
        {
            uDelagate -= PopUp;
            uDelagate += SetScale;
            hasOverReachedY = false;
            hasOverReachedX = false;

        }

        transform.localScale = new Vector3(scaleX, scaleY, defaultScaleUp);

        
    }

    public void AddPopDown() 
    {
        uDelagate += PopDown;
        startPopTime = Time.time;
    }
    
    void PopDown()
    {
        float scaleX;
        float scaleY;
        float scaleZ = 1f;

        scaleX = Mathf.Pow((0.5f - (Time.time - startPopTime)) * scaleSpeedUp, 3f);
        scaleY = Mathf.Pow((0.5f - (Time.time - startPopTime)) * scaleSpeedUp, 3f);

        if ( scaleX <= 0f && scaleY <= 0f)
        {
            scaleX = 0f;
            scaleY = 0f;
            uDelagate -= PopDown;
            uDelagate -= SetScale;
            scaleZ = 0f;
        }


        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    public void ChangeMessage(string m) 
    {
        textbubble = m;
        string temp = "";
        int newLineSize = 24;
        int numberOfLines = 1;
        for (int i = 0; i < textbubble.Length; i++)
        {
            temp += textbubble[i];
            if (i >= newLineSize * numberOfLines && textbubble[i] == ' ')
            {
                temp += '\n';
                numberOfLines++;
            }


        }
        transform.FindChild("Background").localScale = new Vector3(transform.FindChild("Background").localScale.x, transform.FindChild("Background").localScale.y + numberOfLines, transform.FindChild("Background").localScale.z);
        transform.FindChild("Text").GetComponent<TextMesh>().text = temp;
    }
}

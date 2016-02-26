using UnityEngine;
using System.Collections;


enum VectorDirection 
{
    UP,
    FORWARD,
    RIGHT
}
enum WorldSpaces 
{
    LOCAL,
    WORLD
}

public class Rotation_Script : MonoBehaviour {

    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    VectorDirection VectorDirection;
    [SerializeField]
    Space Space_of_Rotation;

    [SerializeField]
    bool isClockwise;

    private float direction;

    Vector3[] directionalVectors = new Vector3[3];

    void Awake() 
    {
        direction = isClockwise?  1f: -1f;

        directionalVectors[0] = Vector3.up;
        directionalVectors[1] = Vector3.forward;
        directionalVectors[2] = Vector3.right;
    }

	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(directionalVectors[(int)VectorDirection], rotationSpeed * Time.deltaTime * direction, Space_of_Rotation);
	}
}

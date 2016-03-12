using UnityEngine;
using System.Collections;

public class SwapMaterialOnTrigger : MonoBehaviour {

    public Material mDefault;
    public Material mSwaped;

    public Renderer renderedMesh;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            renderedMesh.material = mSwaped;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            renderedMesh.material = mDefault;
        }
    }
}

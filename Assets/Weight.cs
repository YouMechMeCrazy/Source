using UnityEngine;
using System.Collections;

public class Weight : MonoBehaviour {

    [SerializeField]
    float weight;

    public float GetWeight()
    {
        return weight;
    }
    public void SetWeight(float newWeight)
    {
        weight=newWeight;
    }
}

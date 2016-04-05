using UnityEngine;
using System.Collections;

public class DestroyBoxAnim : MonoBehaviour
{

    // Use this for initialization
    void Start() {

    StartCoroutine(Die());
 
    }

 IEnumerator Die(){
     yield return new WaitForSeconds(1);
     Destroy(gameObject); 

	}
}
	

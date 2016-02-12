using UnityEngine;
using System.Collections;

public static class Statics  {

    public static RaycastHit[] combine (RaycastHit[] rh1, RaycastHit[] rh2) {

        int a = rh1.Length + rh2.Length;
        RaycastHit[] rh3 = new RaycastHit[a];

        for (int i = 0; i < rh1.Length; i++) { 
            rh3[i] = rh1[i];
                }
        for (int i = 0; i < rh2.Length; i++) {
            rh3[i + rh1.Length] = rh2[i];
        }

        return rh3;


    }


}

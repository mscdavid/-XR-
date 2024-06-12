using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



public class rayinteractor : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit res;
        if(rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            Vector3 groundPt = res.point; // the coordinate that the ray hits
            Debug.Log(" coordinates on the ground: " + groundPt);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

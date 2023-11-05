using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamVectors : MonoBehaviour
{
    [SerializeField] Transform cam;
    public bool showCameraVectors;

    
    void OnDrawGizmos()
    {
        if(showCameraVectors){
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(cam.position , cam.forward);
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(cam.position , cam.right);
        }
    }
}

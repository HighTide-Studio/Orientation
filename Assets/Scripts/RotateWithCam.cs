using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCam : MonoBehaviour
{
    public Transform cam;
    public float moveSpeed = 1.0f;

    void Update()
    {

        // Perform the Slerp with the smoothly increasing ratio
        Quaternion targetRot = cam.rotation;
        //float step = moveSpeed * Time.deltaTime;
        //this.transform.localRotation = Quaternion.RotateTowards(this.transform.localRotation, cam.rotation, step);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, targetRot, moveSpeed * Time.deltaTime);
        //this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, targetRot, 1.0f);
        //this.transform.rotation = cam.rotation;
    }
}
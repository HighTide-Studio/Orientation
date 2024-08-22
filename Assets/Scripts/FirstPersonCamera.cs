using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public PlayerController playerMovement;
    public Transform player;

    public float senX;
    public float senY;
    public float multiplier = 1f;

    public Transform orientation;
    [SerializeField] private Quaternion curRotation;
    public float rotChangeSpeed = 10f;

    public float xRot;
    public float yRot;
    public float zRot = 0f;
    public float targetZRot = 0f;
    public float lerpSpeed = 1f;
    [SerializeField] public float mouseXInput;
    [SerializeField] public float mouseYInput;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isStrafing)
        {
            //zRot = -playerMovement.horizontalInput * zRot;
        }
        float mouseX = (Input.GetAxis("Mouse X")) * Time.deltaTime * senX * multiplier;
        mouseXInput = mouseX;
        float mouseY = (Input.GetAxis("Mouse Y")) * Time.deltaTime * senY * multiplier;
        mouseYInput = mouseY;

        if (true)
        {
            yRot += mouseX;
            xRot -= mouseY;
        }
        else
        {
            yRot -= mouseX;
            xRot += mouseY;
        }

        xRot = Mathf.Clamp(xRot, -90f, 90f);
        zRot = Mathf.Lerp(zRot, targetZRot, Time.deltaTime * lerpSpeed);
        Quaternion targetRotation = player.rotation;
        curRotation = Quaternion.Slerp(curRotation, targetRotation, rotChangeSpeed * Time.deltaTime);
        transform.rotation = curRotation * Quaternion.Euler(xRot, yRot, zRot);
        orientation.rotation = curRotation * Quaternion.Euler(0, yRot, 0);
    }
}

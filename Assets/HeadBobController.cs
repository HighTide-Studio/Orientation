using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool enable = true;
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30f)] private float frequency = 10.0f;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    public float curSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform camera;
    public float posRef;
    private float directionModVal = 0.0f;
    private float prevValue = 0f;
    private float prevVelocity = 0f;
    private float deltaTime;
    [SerializeField] private float curAcceleration;

    //Other Scripts
    [SerializeField] private PlayerController player;


    void Awake()
    {
        startPos = camera.localPosition;
    }

    void Update()
    {
        if (!enable) return;
        deltaTime = Time.deltaTime;

        CheckMotion();
        ResetPosition();
    }

    //This is gonna represent IDLE BOBBING for breathing
    private Vector3 FootStepMotion()
    {

        if (player.isSprinting)
        {
            directionModVal = 6.0f;
            amplitude = 0.0003f;
            frequency = 15.0f;
        }
        else if (player.isWalking)
        {
            directionModVal = 4.0f;
            amplitude = 0.0002f;
            frequency = 12.0f;
        }
        else
        {
            directionModVal = 1.0f;
            amplitude = 0.0001f;
            frequency = 2.0f;
        }


        //Can later change with noise
        Vector3 pos = Vector3.zero;
        float directionModifier = Mathf.Sign(Mathf.Sin(Time.time * frequency)) < 0 ? directionModVal : 1.0f;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude * directionModifier;


        //Accerlation for Shake
        float curVal = pos.y;
        float deltaValue = pos.y - prevValue;
        float velocity = deltaValue / deltaTime;
        float acceleration = (velocity - prevVelocity) / deltaTime;

        prevValue = curVal;
        prevVelocity = velocity;

        posRef = (1.0f - Mathf.Abs((acceleration * 10000f)));
        //pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2; // this makes it look weird
        return pos;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude;
        curSpeed = speed;
        if (speed > 100f) return;
        if (!player.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }
}

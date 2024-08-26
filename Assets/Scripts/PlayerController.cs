using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool isStrafing;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode lightKey = KeyCode.F;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool isGrounded;

    [Header("Player Step Climb")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    [Header("Physics")]
    [SerializeField] public float gravity = 20f;
    [SerializeField] public Vector3 globalGravityDir; //Always down
    [SerializeField] public Vector3 curGravityDir; //Current Dir
    [SerializeField] public Vector3 gravityVector;
    [SerializeField] public Vector3 curGravityVector; // for lerping
    [SerializeField] private ConstantForce cForce;
    [SerializeField] private float verticalSpeed = 0f;
    [SerializeField] private float jumpSpeed = 0f;
    [SerializeField] private float planetRotateSpeed = 10f;
    [SerializeField] private GravityField curGravityField;
    [SerializeField] private Collider lastCollider;

    [Header("Flashlight")]
    [SerializeField] private GameObject flashlight;
    [SerializeField] private bool isLightOn = false;

    public bool isHolding;
    public bool isWalking;
    public bool isSprinting;

    [Header("Camera Stuff")]
    [SerializeField] private FirstPersonCamera playerCamera;

    Vector3 moveDir;
    Vector3 jumpDir;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        cForce = GetComponent<ConstantForce>();
        isHolding = false;
        isWalking = false;
        isSprinting = false;
        globalGravityDir = Vector3.down;
        curGravityDir = globalGravityDir;
        gravityVector = curGravityDir * gravity;
        curGravityVector = gravityVector;
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, curGravityDir, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        SpeedControl();



        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
            //Vector3 targetGravityVector = gravityVector;
            //curGravityVector = Vector3.Lerp(curGravityVector, targetGravityVector, 1f * Time.deltaTime);
            //rb.velocity += curGravityVector * Time.deltaTime; // This is the gravity line
            rb.velocity += gravityVector * Time.deltaTime; // This is the gravity line
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        //stepClimb();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0f)
        {
            isStrafing = true;
        }
        else
        {
            isStrafing = false;
        }

        if (Input.GetKey(jumpKey) && readyToJump && (isGrounded))
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(lightKey))
        {
            isLightOn = !isLightOn;
            flashlight.SetActive(isLightOn);
        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Orientation
        Vector3 upDir = -curGravityDir.normalized;
        
        //Project on Plane makes a vector3 by projecting the player's forward direction onto the plane perpendicular to gravity
        Vector3 forwardDir = Vector3.ProjectOnPlane(transform.forward, curGravityDir);
        Quaternion targetRotation = Quaternion.LookRotation(forwardDir, upDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, planetRotateSpeed * Time.fixedDeltaTime);

        if (isGrounded)
        {
            verticalSpeed = 0f;
            if (Input.GetKey(runKey))
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 50f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        else if (!(isGrounded) && (gravity != 0f))
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        if (moveDir.magnitude > 0.1f && Input.GetKey(runKey))
        {
            isWalking = false;
            isSprinting = true;
        }
        else if (moveDir.magnitude > 0.1f && !Input.GetKey(runKey))
        {
            isWalking = true;
            isSprinting = false;
        }
        else
        {
            isWalking = false;
            isSprinting = false;
        }
    }

    private void SpeedControl()
    {

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(-curGravityDir * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, orientation.TransformDirection(Vector3.forward), out hitLower, 1f))
        {
            Debug.Log("Low hit");
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, orientation.TransformDirection(Vector3.forward), out hitUpper, 0.5f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.Log("High hit");
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //Trigger Enter Stuff
        if (collision.gameObject.tag == "Gravity")
        {
            Debug.Log("Gravity Enter");
            GravityField gravityField = collision.gameObject.GetComponent<GravityField>();
            curGravityField = gravityField;
            lastCollider = collision;
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Gravity" && lastCollider == collision)
        {
            gravityVector = curGravityField.GetGravityVector(this.gameObject.transform);
            curGravityDir = curGravityField.gravityDir;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        //Trigger Exit Stuff
        if (collision.gameObject.tag == "Gravity")
        {
            Debug.Log("Gravity Exit");
            curGravityField = null;
            lastCollider = null;
            curGravityDir = globalGravityDir;
            gravityVector = curGravityDir * gravity;
        }
    }
}

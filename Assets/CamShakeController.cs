using UnityEngine;

public class CamShakeController : MonoBehaviour
{
    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one * 15;

    [SerializeField]
    float frequency = 25f;

    //public Transform camera;
    private Quaternion startRot;
    private float seed;
    public float shakeSpeed = 1f;
    public Transform cam;

    //Other Scripts
    [SerializeField] private HeadBobController posRef;
    [SerializeField] private PlayerController player;
    [SerializeField] private FirstPersonCamera fpsCam;

    void Awake()
    {
        seed = Random.value;
        startRot = transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //This is getting the arc of the Sin wave from the idle breathing bobbing in HeadBobController
        //float posRefValue = Map(posRef.posRef, 0.0f, 1.0f, 0.5f, 1.0f);

        if (player.isSprinting)
        {
            maximumAngularShake.x = 20.0f;
            maximumAngularShake.y = 30.0f;
            maximumAngularShake.z = 20.0f;
            frequency = 5.0f;
            shakeSpeed = 5f;
        }
        else if (player.isWalking)
        {
            maximumAngularShake.x = 7.0f;
            maximumAngularShake.y = 10.0f;
            maximumAngularShake.z = 7.0f;
            frequency = 5.0f;
            shakeSpeed = 10f;
        }
        else
        {
            maximumAngularShake.x = 3.0f;
            maximumAngularShake.y = 3.0f;
            maximumAngularShake.z = 3.0f;
            frequency = 10.0f;
            shakeSpeed = 5f;
        }

        //float mouseMultiplier = Mathf.Sqrt(fpsCam.mouseXInput * fpsCam.mouseXInput + fpsCam.mouseYInput * fpsCam.mouseYInput) * 5f;
        //float mouseMod = Mathf.Clamp(mouseMultiplier, 1.0f, 2.0f);

        Quaternion noiseRot = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
            ));

        // Get the desired rotation relative to the transform's forward direction
        //Quaternion relativeRotation = Quaternion.LookRotation(cam.forward) * noiseRotation;

        // Convert the world space rotation to local space
        //Quaternion localRotation = Quaternion.Inverse(transform.rotation) * relativeRotation;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, noiseRot, Time.deltaTime * shakeSpeed);
    }

    public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // Ensure that the original range is not zero to avoid division by zero.
        if (fromMax - fromMin == 0)
        {
            return toMin;
        }

        // Normalize the value within the original range.
        float normalizedValue = (value - fromMin) / (fromMax - fromMin);

        // Scale it to the target range.
        return toMin + normalizedValue * (toMax - toMin);
    }
}

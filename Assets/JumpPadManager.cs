using UnityEngine;
using TMPro;

public class JumpPadManager : MonoBehaviour, IInteractable
{
    public bool isPadActivated = false;
    public float padForce = 10f;
    public MeshRenderer jumpPadLightMesh;
    public Material lightMaterial;

    [Header("Prompt Text")]
    [SerializeField] private TextMeshProUGUI prompt;
    [SerializeField] private TextMeshProUGUI key;

    void Start()
    {
        //Makes a quick copy of the light pad's current material
        jumpPadLightMesh = this.transform.parent.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        lightMaterial = new Material(jumpPadLightMesh.material);
        lightMaterial.DisableKeyword("_EMISSION");
        jumpPadLightMesh.material = lightMaterial;
        key.text = "E";
    }

    public void Interact()
    {
        isPadActivated = !isPadActivated;
        if (isPadActivated)
        {
            //Debug.Log("ON");
            lightMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            //Debug.Log("OFF");
            lightMaterial.DisableKeyword("_EMISSION");
        }
        jumpPadLightMesh.material = lightMaterial;
        Debug.Log("Player interacted with jump pad");
    }

    public void LookAt()
    {
        if (isPadActivated)
        {
            prompt.text = "ON";
        }
        else
        {
            prompt.text = "OFF";
        }
    }
}

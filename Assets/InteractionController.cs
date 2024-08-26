using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [Header("Interaction Parameters")]
    [SerializeField] private float interactionRange = 5.0f;

    [Header("GUI Prompt")]
    [SerializeField] private GameObject interactablePrompt;

    void Start()
    {
        interactablePrompt.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactionRange))
            {
                if (hit.collider.gameObject.CompareTag("Interactable")) // not needed
                {
                    IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactionRange))
        {
            if (hit.collider.gameObject.CompareTag("Interactable")) // not needed
            {
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactablePrompt.SetActive(true);
                    interactable.LookAt();
                }
            }
            else
            {
                interactablePrompt.SetActive(false);
            }
        }
        else
        {
            interactablePrompt.SetActive(false);
        }
    }
}

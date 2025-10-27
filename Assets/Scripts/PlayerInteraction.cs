using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    [SerializeField] private Inventory inventory;
    private Interactable currentInteractable;

    private void Update()
    {
        CheckInteraction();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && currentInteractable != null)
        {
            currentInteractable.Interact();
            Debug.Log($"Interacted with: {currentInteractable.gameObject.name}");
        }
    }

    private void CheckInteraction()
    {
        RaycastHit hit;
        var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable") // if looking at an interactable object
            {
                var newInteractable = hit.collider.GetComponent<Interactable>();

                if (newInteractable.enabled)
                {
                    if (currentInteractable != newInteractable)
                    {
                        DisableCurrentInteractable();
                        SetNewCurrentInteractable(newInteractable);
                    }
                }
                else //if new interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else // if not interactable
            {
                DisableCurrentInteractable();
            }
        }
        else // if nothing in reach
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable interactable)
    {
        currentInteractable = interactable;
        currentInteractable.EnableOutline();
        GUIController.Instance.EnableInteractionText(currentInteractable.message);
    }

    private void DisableCurrentInteractable()
    {
        GUIController.Instance.DisableInteractionText();
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string message;

    public UnityEvent OnInteract;
    private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }

    public void DisableOutline()
    {
        if (outline != null) outline.enabled = false;
    }

    public void EnableOutline()
    {
        if (outline != null) outline.enabled = true;
    }
}
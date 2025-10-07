using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private Outline outline;
    public string message;
    
    public UnityEvent OnInteract;
    
    void Start()
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
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    public void EnableOutline()
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
}

using Generals;
using TMPro;
using UnityEngine;

public class GUIController : Singleton<GUIController>
{
    [SerializeField] private TMP_Text interactionText;

    public void EnableInteractionText(string text)
    {
        interactionText.text = text + " (E)";
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
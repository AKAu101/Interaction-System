using UnityEngine;
using TMPro;
using Generals;

public class GUIController : Singleton<GUIController>
{
    [SerializeField] TMP_Text interactionText;
    //public static GUIController instance;

    //private void Awake()
    //{
    //    instance = this;
    //}

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

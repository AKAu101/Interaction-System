using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ItemContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button consumeButton;
    [SerializeField] private Button dropButton;

    private int currentSlotIndex;
    private ItemView currentItemView;

    private void Awake()
    {
        // Hide menu by default
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // Setup button listeners (functionality will be added later)
        if (consumeButton != null)
        {
            consumeButton.onClick.AddListener(OnConsumeClicked);
        }

        if (dropButton != null)
        {
            dropButton.onClick.AddListener(OnDropClicked);
        }
    }

    private void Update()
    {
        // Close menu if clicking outside of it (using new Input System)
        if (menuPanel.activeSelf && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!IsPointerOverMenu())
            {
                HideMenu();
            }
        }
    }

    private bool IsPointerOverMenu()
    {
        if (EventSystem.current == null || menuPanel == null)
            return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Mouse.current.position.ReadValue();

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == menuPanel || result.gameObject.transform.IsChildOf(menuPanel.transform))
            {
                return true;
            }
        }

        return false;
    }

    public void ShowMenu(Vector3 position, int slotIndex, ItemView itemView)
    {
        if (menuPanel != null)
        {
            currentSlotIndex = slotIndex;
            currentItemView = itemView;

            // Position the menu at the cursor
            menuPanel.transform.position = position;
            menuPanel.SetActive(true);
        }
    }

    public void HideMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            currentSlotIndex = -1;
            currentItemView = null;
        }
    }

    private void OnConsumeClicked()
    {
        Debug.Log($"Consume clicked for slot {currentSlotIndex}");
        // Functionality will be added later
        HideMenu();
    }

    private void OnDropClicked()
    {
        Debug.Log($"Drop clicked for slot {currentSlotIndex}");
        // Functionality will be added later
        HideMenu();
    }
}

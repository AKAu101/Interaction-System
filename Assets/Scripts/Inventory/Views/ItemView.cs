using Generals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private LayerMask dropAreaLayer;
    private GameObject dragStartParent;

    private Vector3 dragStartPosition;

    public int CurrentSlotIndex { get; private set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag on ItemView");

        wrapper.SetActive(true);
        dragStartPosition = transform.position;
        dragStartParent = transform.parent.gameObject;
        transform.SetParent(InventoryGUI.Instance.gameObject.transform);
        transform.position = MouseUtil.GetRawMouse();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = MouseUtil.GetRawMouse();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag on ItemView");

        var origin = transform.position;
        var direction = Vector3.forward;
        var raycastDistance = 100f;

        Debug.DrawRay(origin, transform.forward * raycastDistance, Color.red, 2f);

        if (Physics.Raycast(transform.position, transform.forward, out var hit, raycastDistance, dropAreaLayer))
        {
            Debug.Log("Drop area hit");

            var slot = hit.transform.gameObject.GetComponent<SlotView>().Slot;
            Debug.Log($"EndDragSlot:{slot}");
            if (!Inventory.Instance.TryMoveItem(CurrentSlotIndex, slot))
            {
                transform.SetParent(dragStartParent.transform);
                transform.position = dragStartPosition;
            }
        }
        else
        {
            // No drop area hit, return to original position
            transform.SetParent(dragStartParent.transform);
            transform.position = dragStartPosition;
        }
    }


    public void Setup(ItemSO itemType, int slot, int amount)
    {
        image.sprite = itemType.icon;
        CurrentSlotIndex = slot;
        UpdateAmount(amount);
    }

    public void UpdateAmount(int amount)
    {
        if (countText != null)
        {
            if (amount > 1)
            {
                countText.text = amount.ToString();
                countText.gameObject.SetActive(true);
            }
            else
            {
                countText.gameObject.SetActive(false);
            }
        }
    }

    public void SetReferencedSlot(int slot)
    {
        CurrentSlotIndex = slot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check for right-click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Right-clicked on item in slot {CurrentSlotIndex}");

            // Find the context menu in the scene
            var contextMenu = FindObjectOfType<ItemContextMenu>();
            if (contextMenu != null)
            {
                // Show menu at mouse position
                contextMenu.ShowMenu(MouseUtil.GetRawMouse(), CurrentSlotIndex, this);
            }
            else
            {
                Debug.LogWarning("ItemContextMenu not found in scene!");
            }
        }
    }
}
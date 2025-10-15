using Generals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private Image image;
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
            transform.SetParent(dragStartParent.transform);
            transform.position = dragStartPosition;
        }
    }


    public void Setup(ItemSO itemType, int slot)
    {
        image.sprite = itemType.icon;
        CurrentSlotIndex = slot;
    }

    public void SetReferencedSlot(int slot)
    {
        CurrentSlotIndex = slot;
    }
}
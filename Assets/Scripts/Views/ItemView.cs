using System;
using UnityEngine;
using UnityEngine.UI;
using Generals;
using UnityEngine.EventSystems;

public class ItemView : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] GameObject wrapper;
    [SerializeField] private Image image;
    [SerializeField] LayerMask dropAreaLayer;
    private int referencedSlot;
    
    public int ReferencedSlot => referencedSlot;
    
    //ondrag zeug
    private Vector3 dragStartPos;
    private GameObject dragStartParent;

    
    
    public void Setup(ItemSO itemType,int slot)
    {
        //spriteRenderer.sprite = itemType.icon;
        image.sprite = itemType.icon;
        referencedSlot = slot;

    }
    
    public void SetReferencedSlot(int slot)
    {
        referencedSlot = slot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Mouse down on ItemVIew");
        //if(!Interactions.Instance.PlayerCanInteract()) return;
        //if (!CanInteract) return;

        //Interactions.Instance.PlayerIsDragging = true;
        wrapper.SetActive(true);
        //CardViewHoverSystem.Instance.Hide();
        dragStartPos = transform.position;
        dragStartParent = this.transform.parent.gameObject;
        transform.SetParent(InventoryGUI.Instance.gameObject.transform);
        //dragStartRot = transform.rotation;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetRawMouse(); //MouseUtil.GetMousePositionInWorldSpace(1); 
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = MouseUtil.GetRawMouse();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Mouse UpAsBUtton on ItemVIew");


        //
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.forward;
        float distance = 100f;

        // Ray sichtbar machen
        Debug.DrawRay(origin, transform.forward*distance, Color.red, 2f); // 2 Sekunden sichtbar
        //

        //ExDebug.Log("Checking if hit",DbgType.Controls | DbgType.Visuals | DbgType.CardSystem,0,true);
        //if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropAreaLayer))
        if (Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, 100f,dropAreaLayer))
        {
            //ExDebug.Log("dropareal layer hit", DbgType.Controls | DbgType.Visuals | DbgType.CardSystem, 0, true);
            Debug.Log("droparea hit");
            
            // hit == slot
            //-> slot zu inventar matchen
            var slot = hit.transform.gameObject.GetComponent<SlotView>().Slot;
            Debug.Log($"EndDragSlot:{slot}");
            if (!Inventory.Instance.TryMoveItem(referencedSlot, slot)) {
                transform.SetParent(dragStartParent.transform);
                transform.position = dragStartPos;
            }
        }
        else
        {
            transform.SetParent(dragStartParent.transform);
            transform.position = dragStartPos;
            
            //transform.rotation = dragStartRot;
        }
        //Interactions.Instance.PlayerIsDragging = false;
    }
}

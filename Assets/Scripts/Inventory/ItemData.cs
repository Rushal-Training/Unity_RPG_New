using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public int amount = 1;

    private Transform originalParent;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.ID != -1)
        {
            originalParent = this.transform.parent;
            this.transform.SetParent(this.transform.parent.parent);
            this.transform.position = eventData.position;

            Debug.Log ("clicked item");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item.ID != -1)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(originalParent);
    }    
}
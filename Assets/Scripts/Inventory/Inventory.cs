using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject inventoryPanel;
    GameObject slotPanel;
    ItemDatabase database;
    [SerializeField] GameObject inventorySlot;
    [SerializeField] GameObject inventoryItem;

    private int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        database = GetComponent<ItemDatabase>();

        slotAmount = 16;
        inventoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = inventoryPanel.transform.Find("Slot Panel").gameObject;

        for ( int i = 0; i < slotAmount; i++ )
        {
            items.Add(new Item());
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }

        AddItem(0);
        AddItem(1);
        AddItem(1);
        AddItem(1);
    }

    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);
        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;
                    GameObject itemObject = Instantiate(inventoryItem);
                    itemObject.GetComponent<ItemData>().item = itemToAdd;
                    itemObject.transform.SetParent(slots[i].transform);
                    itemObject.transform.position = Vector2.zero;
                    itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObject.name = itemToAdd.Title;
                    break;
                }
            }
        }
    }

       

    private bool CheckIfItemIsInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if ( items[i].ID == item.ID )
            {
                return true;
            }
        }
        return false;
    }
}
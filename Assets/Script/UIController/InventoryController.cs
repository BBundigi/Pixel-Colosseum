using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
    public static InventoryController Instance;
    [SerializeField]
    private GameObject InventoryPanel;

    private List<int> InventoryItemIDs;
    private Slot[] Slots;

    private void Awake()
    {
        if (Instance == null)
        {
            Slots = GetComponentsInChildren<Slot>(true);
            InventoryItemIDs = new List<int>();
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OpenInventory()
    {
        InventoryPanel.SetActive(true);
        ResetSlotData();
    }

    public void ColoseInventory()
    {
        InventoryPanel.SetActive(false);
    }

    public void SetInventoryList(int TargetItemID)
    {
        InventoryItemIDs.Add(TargetItemID);
    }

    public void RemoveInventoryList(int TargetIndex)
    {
        InventoryItemIDs.RemoveAt(TargetIndex);
    }

    private void ResetSlotData()
    {
        for(int i =0; i < InventoryItemIDs.Count; i++)
        {
            Slots[i].SetSprite(ItemDatas.ItemSprites[InventoryItemIDs[i]]);
        }
    }

}

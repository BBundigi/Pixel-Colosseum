using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    public static InventoryController Instance;
    [SerializeField]
    private GameObject InventoryPanel;
    [SerializeField]
    private Button InventoryOpenButton;
    private List<eItemID> InventoryItemIDs;
    private Slot[] Slots;

    private void Awake()
    {
        if (Instance == null)
        {
            Slots = GetComponentsInChildren<Slot>(true);
            InventoryItemIDs = new List<eItemID>();
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
        TouchManager.Instance.NowMode = eTouchMode.CloseInventory;
        InventoryOpenButton.enabled = false;
    }

    public void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        TouchManager.Instance.NowMode = eTouchMode.GamePlay;
        InventoryOpenButton.enabled = true;
    }

    public void CloseInventory_ForThrowing()
    {
        InventoryPanel.SetActive(false);
        InventoryOpenButton.enabled = true;
    }

    public void SetInventoryList(eItemID TargetItemID)
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
            Slots[i].SetSprite(InventoryItemIDs[i]);
        }
    }
}

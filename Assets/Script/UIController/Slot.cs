using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour {

    private int IndexNumber;
    private Image InventoryImage;
    private Button button;
    private eItemID SettedItemID;

    private void Awake()
    {
        InventoryImage = GetComponentInChildren<Image>(true);
        button = GetComponent<Button>();
        button.enabled = false;
        InventoryImage.color = Color.clear;
    }

    public void SetSlot(eItemID ID,int SlotIndex)
    {
        InventoryImage.color = Color.white;
        InventoryImage.sprite = ItemInfoManager.Instance.GetItemSprite(ID);
        SettedItemID = ID;
        button.enabled = true;
        IndexNumber = SlotIndex;
    }

    public void RemoveSlot()
    {
        InventoryImage.color = Color.clear;
        button.enabled = false;
    }

    public void RemoveUsedItem()
    {

    }

    public void ShowExplanation_OnClicked()
    {
        PopUpTextController.Instance.PopUpItemExplanaition(SettedItemID);
    }
}

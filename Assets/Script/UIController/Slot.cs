using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour {

    private Image inventoryImage;
    private Button button;
    private eItemID SettedItem;

    private void Awake()
    {
        inventoryImage = GetComponentInChildren<Image>(true);
        button = GetComponent<Button>();
        button.enabled = false;
        inventoryImage.color = Color.clear;
    }

    public void SetSprite(eItemID item)
    {
        inventoryImage.color = Color.white;
        inventoryImage.sprite = ItemDatas.ItemSprites[(int)item];
        SettedItem = item;
        button.enabled = true;
    }

    public void RemoveSprite()
    {
        inventoryImage.color = Color.clear;
        button.enabled = false;
    }

    public void ShowExplanation_OnClicked()
    {
        PopUpTextController.Instance.PopUpItemExplanaition(SettedItem);
    }
}

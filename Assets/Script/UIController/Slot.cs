using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour {

    private Image inventoryImage;

    private void Awake()
    {
        inventoryImage = GetComponentInChildren<Image>(true);
        inventoryImage.color = Color.clear;
    }

    public void SetSprite(Sprite TargetSprite)
    {
        inventoryImage.color = Color.white;
        inventoryImage.sprite = TargetSprite;
    }

    public void RemoveSprite()
    {
        inventoryImage.color = Color.clear;
    }
}

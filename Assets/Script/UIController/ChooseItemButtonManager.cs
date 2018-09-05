using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseItemButtonManager: MonoBehaviour {
    public static ChooseItemButtonManager Instance;
    [SerializeField]
    private GameObject[] ChooseItemGameObjects;
    [SerializeField]
    private GameObject ChoosItemsParent;

    private ChooseItemButton[] ChoosItemButtons;
    private VoidCallBack ButtonFunction;

    private string GetRandomPotionText;
    private string GetRandomStatusText;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    {
        ChoosItemButtons = new ChooseItemButton[3];
        for(int i=0; i < ChooseItemGameObjects.Length; i++)
        {
            ChoosItemButtons[i] = ChooseItemGameObjects[i].GetComponent<ChooseItemButton>();
        }
        GetRandomPotionText = "Get Three Random Potion";
        GetRandomStatusText = "Upgrade Random Status";
        GenerateButtons();
    }




    public void GenerateButtons()
    {
        ChoosItemsParent.SetActive(true);

        for (int i = 0; i < ChooseItemGameObjects.Length; i++)
        {
            eChooseItemMode itemMode = EnumManager.RandomEnumValue<eChooseItemMode>();
            switch (itemMode)
            {
                case eChooseItemMode.RandomPotion:
                    {
                        Sprite tempSprite = ItemDatas.ItemSprites[ItemDatas.PotionID[Random.Range(0, ItemDatas.PotionID.Length)]];
                        ButtonFunction += Func_RandomPotion;
                        ButtonFunction += CloseButtons;
                        
                        ChoosItemButtons[i].SettingButtons(tempSprite,GetRandomPotionText,ButtonFunction);
                        break;
                    }
                case eChooseItemMode.RandomStatus:
                    {
                        break;
                    }
            }
        }
    }    
    public void CloseButtons()
    {
        ChoosItemsParent.SetActive(false);
    }

    private void Func_RandomPotion()
    {
        for (int i = 0; i < 3; i++)
        {
            int random = ItemDatas.PotionID[Random.Range(0, ItemDatas.PotionID.Length)];

            InventoryController.Instance.SetInventoryList(random);
        }
        TurnManager.Instance.PlayerTurnStart();
    }

    private void Func_RandomStatus()
    {

    }
}

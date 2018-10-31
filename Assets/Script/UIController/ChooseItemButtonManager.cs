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
        TouchManager.Instance.NowMode = eTouchMode.ChooseItem;

        for (int i = 0; i < ChooseItemGameObjects.Length; i++)
        {
            eChooseItemMode m_itemMode = EnumManager.RandomEnumValue<eChooseItemMode>();
            VoidCallBack m_buttonFunction = null;
            switch (m_itemMode)
            {
                case eChooseItemMode.RandomPotion:
                    {
                        Sprite tempSprite = ItemInfoManager.Instance.GetItemSprite(ItemInfoManager.Instance.GetRandomPotionID());
                        m_buttonFunction += Func_RandomPotion;
                        m_buttonFunction += CloseButtons;
                        
                        ChoosItemButtons[i].SettingButtons(tempSprite,GetRandomPotionText, m_buttonFunction);
                        break;
                    }
                //case eChooseItemMode.RandomStatus:
                //    {
                //        break;
                //    }
            }
        }
    }    
    private void CloseButtons()
    {
        ChoosItemsParent.SetActive(false);
    }

    private void Func_RandomPotion()
    {
        for (int i = 0; i < 3; i++)
        {
            eItemID random = ItemInfoManager.Instance.GetRandomPotionID();
            InventoryController.Instance.SetInventoryList(random);
        }
        TurnManager.Instance.PlayerTurnStart();
    }

    private void Func_RandomStatus()
    {

    }
}

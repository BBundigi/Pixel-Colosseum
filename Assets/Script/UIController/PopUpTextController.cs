using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTextController : MonoBehaviour {

    public static PopUpTextController Instance;
    [SerializeField]
    private GameObject ExplanationPanel;
    [SerializeField]
    private RectTransform CanvasTransform;
    [SerializeField]
    private Text Title;
    [SerializeField]
    private Text Explanation;
    [SerializeField]
    private Image Mainsprite;


    [SerializeField]
    private GameObject PotionItemButtons;
    [SerializeField]
    private GameObject StopThrowing;



    private GameObject NowButton;
    private eItemID Object;

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

    public void PopUpItemExplanaition(eItemID Item)
    {
        ExplanationPanel.SetActive(true);

        Title.text = ItemDatas.ItemNameDic[Item];
        Explanation.text = ItemDatas.ItemExplanationDic[Item];
        Mainsprite.sprite = ItemDatas.ItemSprites[(int)Item];

        NowButton = PotionItemButtons;
        Object = Item;
        PotionItemButtons.SetActive(true);
    }

    public void ThrowButton_OnClicked()
    {
        TouchManager.Instance.TargetItem = Object;
        NowButton.SetActive(false);
        NowButton = null;
        ExplanationPanel.SetActive(false);
        StopThrowing.SetActive(true);
    }

    public void StopThrowing_OnClicked()
    {
        TouchManager.Instance.NowMode = eTouchMode.GamePlay;
        StopThrowing.SetActive(false);
    }
     
    public void Close_OnClicked()
    {
        NowButton.SetActive(false);
        NowButton = null;
        ExplanationPanel.SetActive(false);
    }
}

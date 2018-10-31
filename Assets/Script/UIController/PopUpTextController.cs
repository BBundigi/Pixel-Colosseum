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


    private eItemID  TargetObject;
    private GameObject NowButton;

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

    public void PopUpItemExplanaition(eItemID ID)
    {
        ExplanationPanel.SetActive(true);

        ItemInfo m_TargetItem = ItemInfoManager.Instance.GetItemInfo(ID);

        Title.text = m_TargetItem.ItemName;
        Explanation.text = m_TargetItem.ItemExplanation;
        Mainsprite.sprite = ItemInfoManager.Instance.GetItemSprite(ID);

        NowButton = PotionItemButtons;
        PotionItemButtons.SetActive(true);
    }

    public void ThrowButton_OnClicked()
    {
        TouchManager.Instance.TargetItem = TargetObject;
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

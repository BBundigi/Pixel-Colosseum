using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemDatas
{

    public static Sprite[] ItemSprites = Resources.LoadAll<Sprite>("Sprites/Item&Icon/items");
    public static Dictionary<eItemID, string> ItemNameDic;
    public static Dictionary<eItemID, string> ItemExplanationDic;

    public readonly static eItemID[] PotionIDs = new eItemID[2]
    {
       eItemID.FireFlask,
       eItemID.PoisonFlask,
    };

    public readonly static eItemID[] ScrollIDs = new eItemID[1]
    {
        eItemID.MirrorScroll,
    };

    public static eItemID GetRandomPotionID()
    {
        return PotionIDs[Random.Range(0, PotionIDs.Length)];
    }

    public static eItemID GetRnadomScrollID()
    {
        return ScrollIDs[Random.Range(0, ScrollIDs.Length)];
    }

    static ItemDatas()
    {
        ItemNameDic = new Dictionary<eItemID, string>();
        ItemExplanationDic = new Dictionary<eItemID, string>();
        ItemNameDic.Add(eItemID.FireFlask,sItemName.FIRE_FLASK);
        ItemExplanationDic.Add(eItemID.FireFlask, sItemExplanation.FIRE_FLASK_EXPLANATION);
        ItemNameDic.Add(eItemID.PoisonFlask,sItemName.POISON_FLASK);
        ItemExplanationDic.Add(eItemID.PoisonFlask, sItemExplanation.FIRE_FLASK_EXPLANATION);
        ItemNameDic.Add(eItemID.MirrorScroll, "거울 분신의 주문서");
    }

}

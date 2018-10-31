using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoManager : InformationLoader
{
    public static ItemInfoManager Instance;
    private Dictionary<eItemID, ItemInfo> ItemInfoDic;
    private Dictionary<eItemID, Sprite> ItemSpriteDic;

    private eItemID[] PotionIDs = new eItemID[2]
    {
       eItemID.FireFlask,
       eItemID.PoisonFlask,
    };

    private eItemID[] ScrollIDs = new eItemID[1]
    {
        eItemID.MirrorScroll,
    };

    public eItemID GetRandomPotionID()
    {
        return PotionIDs[Random.Range(0, PotionIDs.Length)];
    }

    public eItemID GetRandomScrollID()
    {
        return ScrollIDs[Random.Range(0, ScrollIDs.Length)];
    }

    public ItemInfo GetItemInfo(eItemID ID)
    {
        return ItemInfoDic[ID];
    }

    public Sprite GetItemSprite(eItemID ID)
    {
        return ItemSpriteDic[ID];
    }

    private void Awake()
    {
        ItemInfo[] ItemInfoArr;

        Load(out ItemInfoArr, "JsonFiles/ItemInfos");

        for(int i =0; i < ItemInfoArr.Length; i++)
        {
            ItemInfoDic.Add(ItemInfoArr[i].ItemID, ItemInfoArr[i]);
        }

        Sprite[] ItemSprites = Resources.LoadAll<Sprite>("Sprites/Item&Icon/items");

        for(int i = 0; i< ItemSprites.Length; i++)
        {
            ItemSpriteDic.Add((eItemID)i, ItemSprites[i]);
        }

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }  
}

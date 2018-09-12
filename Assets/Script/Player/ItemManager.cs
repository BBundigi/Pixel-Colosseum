using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager {
    private static GameObject FlaskTemplate = (GameObject)Resources.Load("Prefab/FlaskTemplate");

    public static void UseItem(int LocX, int LocY, eItemID TargetItem)
    {
        switch (TargetItem)
        {
            case eItemID.FireFlask:
            case eItemID.PoisonFlask:
                {
                    GameObject NewFlask = GameObject.Instantiate(FlaskTemplate);

                    NewFlask.GetComponent<FlaskManager>().SetFlask(LocX, LocY, TargetItem);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
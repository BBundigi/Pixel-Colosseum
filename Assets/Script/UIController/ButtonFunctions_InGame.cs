using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions_InGame : MonoBehaviour {
    public delegate void VoidCallBack();
    private VoidCallBack ToolBarCallBack;//How To Use delegate
    public void TurnEndButton()
    {
        TurnManager.Instance.PlayerTurnEnd();
    }

    public void AskTileButton()
    {
        //TouchSystem Change
    }

    public void UserToolBar(VoidCallBack CallBack)
    {

    }
}

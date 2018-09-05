using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChooseItemButton : MonoBehaviour {
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;

    public void SettingButtons(Sprite TargetSprite, string Text, VoidCallBack Callback)
    {
        UnityAction TargetAction = new UnityAction(Callback);
        button.onClick.AddListener(TargetAction);
        text.text = Text;
        image.sprite = TargetSprite;
    }
}

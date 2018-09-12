using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIManager : MonoBehaviour {

    public static PlayerStatusUIManager Instance;
    [SerializeField]
    private GameObject PlayerStatus;

    private Slider HPBar;

    private void Awake()
    {
        if (Instance == null)
        {
            HPBar = PlayerStatus.GetComponentInChildren<Slider>();
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetHP(float MaxHP, float CurrentHP)
    {
        HPBar.value = CurrentHP / MaxHP;
    }
}

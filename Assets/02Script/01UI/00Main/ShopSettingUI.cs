using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSettingUI : MonoBehaviour
{
    public GameObject MileageWindow;
    //public Image MileageImage;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI ticketText;

    public TextMeshProUGUI[] diamondPayTexts;
    public TextMeshProUGUI[] goldPayTexts;
    public TextMeshProUGUI[] ticketPayTexts;
    public TextMeshProUGUI[] mileagePayTexts;
    public TextMeshProUGUI[] diamondBonusTexts;
    public TextMeshProUGUI[] goldBonusTexts;
    
    private void Start()
    {
        diamondPayTexts[0].text=DataTableManager.Get<StringTable>(DataTableIds.String).Get(90051);
        
    }

    private void Update()
    {
        UpdatePlayerInfo();
    }

    private void UpdatePlayerInfo()
    {
        if (goldText)
        {
            goldText.text = GameManager.instance.Gold.ToString();    
        }

        if (diamondText)
        {
            diamondText.text = GameManager.instance.Diamond.ToString();
        }
        
        if(ticketText)
        {
            ticketText.text = MileageWindow.activeSelf ? GameManager.instance.Mileage.ToString() : GameManager.instance.Ticket.ToString();
        }
    }
    
    
}

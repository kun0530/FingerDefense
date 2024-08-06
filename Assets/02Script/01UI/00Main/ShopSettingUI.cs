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
    
    private ShopTable shopTable;
    
    private bool dataLoaded = false;
    private void OnEnable()
    {
        if (!dataLoaded)
        {
            LoadData();
            dataLoaded = true;
        }
    }
    private void LoadData()
    {
        shopTable = DataTableManager.Get<ShopTable>(DataTableIds.Shop);
        if (shopTable == null)
        {
            Logger.LogError("ShopTable is null");
            return;
        }
        SetTextElements();
    }
    private void SetTextElements()
    {
        diamondPayTexts[0].text=shopTable.Get(90051);
        diamondPayTexts[1].text=shopTable.Get(90061);
        diamondPayTexts[2].text=shopTable.Get(90081);
        diamondPayTexts[3].text=shopTable.Get(90101);
        diamondPayTexts[4].text=shopTable.Get(90121);
        diamondPayTexts[5].text=shopTable.Get(90141);
        goldPayTexts[0].text=shopTable.Get(90161);
        goldPayTexts[1].text=shopTable.Get(90171);
        goldPayTexts[2].text=shopTable.Get(90181);
        goldPayTexts[3].text=shopTable.Get(90191);
        goldPayTexts[4].text=shopTable.Get(90201);
        goldPayTexts[5].text=shopTable.Get(90211);
        
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

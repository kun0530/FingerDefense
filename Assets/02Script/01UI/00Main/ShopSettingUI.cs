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
    public TextMeshProUGUI[] itemTitleTexts;
    public TextMeshProUGUI[] ticketPayTexts;
    public TextMeshProUGUI[] mileagePayTexts;
    public TextMeshProUGUI[] diamondBonusTexts;
    public TextMeshProUGUI[] goldBonusTexts;
    
    public TextMeshProUGUI[] tapMenuTexts;
    
    private StringTable shopTable;
    
    private bool dataLoaded = false;
    private void OnEnable()
    {
        if (!dataLoaded)
        {
            LoadData();
            dataLoaded = true;
        }
        GameManager.instance.OnResourcesChanged += UpdatePlayerInfo;
        UpdatePlayerInfo();
    }
    private void LoadData()
    {
        shopTable = DataTableManager.Get<StringTable>(DataTableIds.String);
        if (shopTable == null)
        {
            Logger.LogError("ShopTable is null");
            return;
        }
        SetTextElements();
    }
    private void SetTextElements()
    {
        tapMenuTexts[0].text=shopTable.Get(90581.ToString());
        tapMenuTexts[1].text=shopTable.Get(90591.ToString());
        tapMenuTexts[2].text=shopTable.Get(90601.ToString());
        tapMenuTexts[4].text=shopTable.Get(90611.ToString());
        
        diamondPayTexts[0].text=shopTable.Get(90051.ToString());
        diamondPayTexts[1].text=shopTable.Get(90061.ToString());
        diamondPayTexts[2].text=shopTable.Get(90081.ToString());
        diamondPayTexts[3].text=shopTable.Get(90101.ToString());
        diamondPayTexts[4].text=shopTable.Get(90121.ToString());
        diamondPayTexts[5].text=shopTable.Get(90141.ToString());
        
        goldPayTexts[0].text=shopTable.Get(90161.ToString());
        goldPayTexts[1].text=shopTable.Get(90171.ToString());
        goldPayTexts[2].text=shopTable.Get(90181.ToString());
        goldPayTexts[3].text=shopTable.Get(90191.ToString());
        goldPayTexts[4].text=shopTable.Get(90201.ToString());
        goldPayTexts[5].text=shopTable.Get(90211.ToString());
        
        itemTitleTexts[0].text=shopTable.Get(90221.ToString());
        itemTitleTexts[1].text=shopTable.Get(90231.ToString());
        itemTitleTexts[2].text=shopTable.Get(90241.ToString());
        itemTitleTexts[3].text=shopTable.Get(90251.ToString());
        itemTitleTexts[4].text=shopTable.Get(90261.ToString());
        itemTitleTexts[5].text=shopTable.Get(90271.ToString());
        itemTitleTexts[6].text=shopTable.Get(90281.ToString());
        itemTitleTexts[7].text=shopTable.Get(90291.ToString());
        itemTitleTexts[8].text=shopTable.Get(90301.ToString());
        itemTitleTexts[9].text=shopTable.Get(90311.ToString());
        itemTitleTexts[10].text=shopTable.Get(90321.ToString());
        
        diamondBonusTexts[1].text=shopTable.Get(90071.ToString());
        diamondBonusTexts[2].text=shopTable.Get(90091.ToString());
        diamondBonusTexts[3].text=shopTable.Get(90111.ToString());
        diamondBonusTexts[4].text=shopTable.Get(90131.ToString());
        diamondBonusTexts[5].text=shopTable.Get(90151.ToString());
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

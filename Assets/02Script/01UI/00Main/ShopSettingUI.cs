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
    
    private ShopTable shopTable;
    
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
        tapMenuTexts[0].text=shopTable.Get(90571);
        tapMenuTexts[1].text=shopTable.Get(90581);
        tapMenuTexts[2].text=shopTable.Get(90591);
        tapMenuTexts[4].text=shopTable.Get(90601);
        
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
        
        itemTitleTexts[0].text=shopTable.Get(90221);
        itemTitleTexts[1].text=shopTable.Get(90231);
        itemTitleTexts[2].text=shopTable.Get(90241);
        itemTitleTexts[3].text=shopTable.Get(90251);
        itemTitleTexts[4].text=shopTable.Get(90261);
        itemTitleTexts[5].text=shopTable.Get(90271);
        itemTitleTexts[6].text=shopTable.Get(90281);
        itemTitleTexts[7].text=shopTable.Get(90291);
        itemTitleTexts[8].text=shopTable.Get(90301);
        itemTitleTexts[9].text=shopTable.Get(90311);
        itemTitleTexts[10].text=shopTable.Get(90321);
        
        diamondBonusTexts[1].text=shopTable.Get(90071);
        diamondBonusTexts[2].text=shopTable.Get(90091);
        diamondBonusTexts[3].text=shopTable.Get(90111);
        diamondBonusTexts[4].text=shopTable.Get(90131);
        diamondBonusTexts[5].text=shopTable.Get(90151);
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

using TMPro;
using UnityEngine;

public class CharacterInfoSlot : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHp;
    public TextMeshProUGUI characterAtk;
    public TextMeshProUGUI characterCoolTime;
    public TextMeshProUGUI characterSkillName;
    public TextMeshProUGUI characterSkillDescription;
   
    private StringTable stringTable;
    private PlayerCharacterTable playerCharacterTable;
    private AssetListTable assetListTable;
    
    private void OnEnable()
    {
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoSlot : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHp;
    public TextMeshProUGUI characterAtk;
    public TextMeshProUGUI characterCoolTime;
    public TextMeshProUGUI characterSkillName;
    public TextMeshProUGUI characterSkillDescription;
    
    //스킬이 있을때만 활성화 시킬 목록
    public TextMeshProUGUI characterSkillText;
    public Image SkillImage;
    
    private StringTable stringTable;
    private PlayerCharacterTable playerCharacterTable;
    private AssetListTable assetListTable;
    private SkillTable skillTable;
    
    private void OnEnable()
    {
        stringTable ??= DataTableManager.Get<StringTable>(DataTableIds.String);
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        skillTable ??= DataTableManager.Get<SkillTable>(DataTableIds.Skill);
    }

    public void SetCharacterInfo(PlayerCharacterData clickedSlotCharacterData)
    {
        var characterData = playerCharacterTable.Get(clickedSlotCharacterData.Id);

        // 기본 정보 설정
        characterName.text = stringTable.Get(characterData.Name);
        characterHp.text = characterData.Hp.ToString();
        characterAtk.text = skillTable.Get(characterData.Skill1).Damage.ToString();
        characterCoolTime.text = characterData.RespawnCoolTime.ToString();

        // 스킬 정보 설정
        string skillName = stringTable.Get(characterData.SkillName.ToString());
        string skillDescription = stringTable.Get(characterData.SkillText.ToString());
        
        if (!string.IsNullOrEmpty(skillName) && !string.IsNullOrEmpty(skillDescription) && characterData.SkillName != 0 && characterData.SkillText != 0)
        {
            characterSkillName.text = skillName;
            characterSkillDescription.text = skillDescription;
            characterSkillText.gameObject.SetActive(true);
        }
        else
        {
            // 스킬 정보가 없을 때 비활성화
            characterSkillName.text = "";
            characterSkillDescription.text = "";
            characterSkillText.gameObject.SetActive(false);
        }
        
        var skillImage = SkillImage.GetComponent<Image>();
        var skillId = assetListTable.Get(characterData.SkillIcon);

        if (skillImage != null && !string.IsNullOrEmpty(skillId))
        {
            var skillSprite = Resources.Load<Sprite>($"Prefab/09SkillIcon/{skillId}");
            if (skillSprite != null)
            {
                skillImage.sprite = skillSprite;
                skillImage.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"Skill sprite not found for skill ID: {skillId}");
                skillImage.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("Skill ID is empty or skillImage is null.");
            skillImage.gameObject.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributeFilter : MonoBehaviour
{
    private PlayerCharacterTable playerCharacterTable;

    private Dictionary<int, PlayerCharacterTable> ElementType= new Dictionary<int, PlayerCharacterTable>();
    private Dictionary<int, PlayerCharacterTable> SkillType= new Dictionary<int, PlayerCharacterTable>();
    private Dictionary<int, PlayerCharacterTable> GradeType= new Dictionary<int, PlayerCharacterTable>();
    
    public RectTransform ElementParent;
    public RectTransform SkillParent;
    public RectTransform GradeParent;
    
    private void Start()
    {
        playerCharacterTable ??= DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        
        //CreateFilteringSlots();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Item/Active Create Dummy", fileName = "Item.asset")]
public class ItemActiveCreateDummy : ActiveItem
{
    [Header("이펙트")]
    public EffectController entryEffectPrefab;
    public EffectController exitEffectPrefab;
    [Header("신의 대리인")]
    public PlayerCharacterController characterPrefab;

    [Header("신의 대리인의 스탯")]
    public float hp;
    private Elements element = Elements.NONE;
    public int skillId1;
    public int skillId2;

    public float moveSpeed;
    public float findRange;
    public float thresholdRange;

    private bool isDragging;
    private InputManager inputManager;
    private DragAndDrop dragAndDrop;

    private SkillTable skillTable;

    private float minY;
    private float maxY;

    private PlayerCharacterController activeAgent;

    public override void Init()
    {
        base.Init();
        isDragging = false;

        var inputSystemGo = GameObject.FindWithTag("InputManager");
        dragAndDrop = inputSystemGo?.GetComponent<DragAndDrop>();
        inputManager = inputSystemGo?.GetComponent<InputManager>();

        var castlePos1Y = StageMgr.castleRightTopPos.position.y;
        var castlePos2Y = StageMgr.castleLeftBottomPos.position.y;
        minY = Mathf.Min(castlePos1Y, castlePos2Y);
        maxY = Mathf.Max(castlePos1Y, castlePos2Y);

        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
    }

    public override void UseItem()
    {
        if (isDragging)
            return;

        isDragging = true;
        if (inputManager)
            inputManager.OnClick += CreateDummy;
    }

    public override void CancelItem()
    {
        base.CancelItem();

        if (activeAgent != null)
        {
            var exitEffect = Instantiate(exitEffectPrefab, activeAgent.transform.position, Quaternion.identity);
            exitEffect.LifeTime = 1f;
            Destroy(activeAgent.gameObject);
        }
    }

    public override void UpdateItem()
    {
        base.UpdateItem();
    }

    private void CreateDummy(InputAction.CallbackContext context)
    {
        isDragging = false;
        if (inputManager)
            inputManager.OnClick -= CreateDummy;

        var spawnPos = Camera.main!.ScreenToWorldPoint(dragAndDrop.GetPointerPosition());
        if (spawnPos.y > maxY || spawnPos.y < minY)
            return;

        base.UseItem();

        var data = new PlayerCharacterData
        {
            Hp = hp,
            Element = (int)element,
            Skill1 = skillId1,
            Skill2 = skillId2
        };

        spawnPos.z = spawnPos.y;
        var instantiatedCharacter = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
        instantiatedCharacter.Status.Data = data;
        
        if (instantiatedCharacter.TryGetComponent<PlayerAttackBehavior>(out var attackBehavior))
        {
            var normalAttackData = skillTable.Get(data.Skill1);
            var skillAttackData = skillTable.Get(data.Skill2);

            var normalAttack = SkillFactory.CreateSkill(normalAttackData, instantiatedCharacter.gameObject);
            var skillAttack = SkillFactory.CreateSkill(skillAttackData, instantiatedCharacter.gameObject);

            attackBehavior.normalAttack = normalAttack;
            attackBehavior.SkillAttack = skillAttack;
        }

        if (instantiatedCharacter.TryGetComponent<DummyMoveBehavior>(out var moveBehavior))
        {
            moveBehavior.moveSpeed = moveSpeed;
            moveBehavior.findRange = findRange;
            moveBehavior.thresholdRange = thresholdRange;

            moveBehavior.Init();
        }

        var entryEffect = Instantiate(entryEffectPrefab, spawnPos, Quaternion.identity);
        entryEffect.LifeTime = 1f;
        activeAgent = instantiatedCharacter;
    }
}

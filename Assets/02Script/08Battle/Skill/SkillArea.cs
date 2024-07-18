using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이디어1. 버프를 줄 때, 따로 저장 -> 버프 리스트에 등록
// 아이디어2. 버프를 줄 때, 버프 리스트에 등록, 타이머를 최솟값으로 설정 -> 타이머를 0으로 설정

public class SkillArea : MonoBehaviour
{
    private SkillData skillData;
    private AreaTargetSkill areaTargetSkill;
    private float timer;

    public Dictionary<IDamageable, Buff> Buffs { get; private set; } = new();

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Start()
    {
        var skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
        skillData = skillTable.Get(1001);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= skillData.Duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryGetTarget(other, out var damageable))
        {
            areaTargetSkill.EnterArea(damageable, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TryGetTarget(other, out var damageable))
        {
            areaTargetSkill.ExitArea(damageable, this);
        }
    }

    private bool TryGetTarget(Collider other, out IDamageable damageable)
    {
        damageable = null;
        switch (skillData.Target)
        {
            case 0:
                if (!other.CompareTag(Defines.Tags.PLAYER_TAG))
                    return false;
                break;
            case 1:
                if (!other.CompareTag(Defines.Tags.MONSTER_TAG))
                    return false;
                break;
            default:
                return false;
        }
        return other.TryGetComponent(out damageable);
    }
}

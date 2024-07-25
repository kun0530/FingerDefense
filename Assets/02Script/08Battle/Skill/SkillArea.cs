using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이디어1. 버프를 줄 때, 따로 저장 -> 버프 리스트에 등록
// 아이디어2. 버프를 줄 때, 버프 리스트에 등록, 타이머를 최솟값으로 설정 -> 타이머를 0으로 설정

public class SkillArea : MonoBehaviour
{
    public PlacementSkill placementSkill;
    public float areaDuration;
    private float timer = 0f;
    private string targetTag;

    public Dictionary<IDamageable, Buff> Buffs { get; private set; } = new();

    public void Init(PlacementSkill skill, SkillData data)
    {
        if (skill == null || data == null)
            return;

        switch (data.Target)
        {
            case 0:
                targetTag = Defines.Tags.PLAYER_TAG;
                break;
            case 1:
                targetTag = Defines.Tags.MONSTER_TAG;
                break;
            default:
                return;
        }

        placementSkill = skill;
        areaDuration = data.Duration;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= areaDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryGetTarget(other, out var damageable))
        {
            placementSkill.EnterArea(damageable, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TryGetTarget(other, out var damageable))
        {
            placementSkill.ExitArea(damageable, this);
        }
    }

    private bool TryGetTarget(Collider other, out IDamageable damageable)
    {
        damageable = null;
        if (!other.CompareTag(targetTag))
            return false;
        
        return other.TryGetComponent(out damageable);
    }
}

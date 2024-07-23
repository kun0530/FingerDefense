using System.Collections;
using System.Collections.Generic;
using Defines;
using UnityEngine;

public static class SkillFactory
{
    public static BaseSkill CreateSkill(SkillData data, GameObject gameObject)
    {
        if (data == null || gameObject == null)
            return null;

        // Target: 타겟 레이어
        LayerMask layerMask = Layers.DEFAULT_LAYER;
        switch (data.Target)
        {
            case 0:
                layerMask = Layers.PLAYER_LAYER;
                break;
            case 1:
                layerMask = Layers.MONSTER_LAYER;
                break;
        }

        // 1차 타겟팅: 0인 경우, 본인 타겟팅(Target 의미 없음) / 그 외는 지정된 타겟팅
        // 1차 타겟팅은 단일 타겟팅
        IFindable primaryTargeting;
        if (data.Center == 0f)
        {
            primaryTargeting = new FindingSelf(gameObject);
        }
        else
        {
            primaryTargeting = new FindingTargetInCircle(gameObject.transform, data.Center, layerMask);
        }

        // 스킬 타입: 0: 단일 / 1: 범위 / 2: 장판
        // To-Do: 스킬 타입이 범위와 장판인 경우, Range를 통해 2차 타겟팅
        // 단일: 2차 타겟팅은 FindingSelf
        // 범위: 2차 타겟팅은 FindingTargetInCircle.FindTargets
        // 장판: Range만큼의 장판 생성!!
        SkillType skillType = null;
        switch ((SkillRangeTypes)data.Type)
        {
            case SkillRangeTypes.SingleTarget:
                skillType = new TargetSkill(new FindingSelf(gameObject), data.AssetNo);
                break;
            case SkillRangeTypes.MultipleTarget:
                skillType = new TargetSkill(new FindingTargetInCircle(gameObject.transform, data.Center, layerMask), data.AssetNo);
                break;
            case SkillRangeTypes.AreaTarget:
                skillType = new AreaTargetSkill(new FindingSelf(gameObject), data.AssetNo); // 미구현
                break;
        }

        // Damage: 공격 스킬 / BuffId: 버프 스킬
        if (data.Damage != 0f)
        {
            // skill.skillActions.Add(new AttackSkill(data.Damage));
            skillType.attackSkill = new AttackSkill(data.Damage);
        }
        if (data.BuffId != 0)
        {
            var buffTable = DataTableManager.Get<BuffTable>(DataTableIds.Buff);
            var buffData = buffTable.Get(data.BuffId);
            // skill.skillActions.Add(new BuffSkill(buffData));
            skillType.buffSkill = new BuffSkill(buffData);
        }

        // To-Do: Instant Skill과 Projectile Skill 분기
        BaseSkill baseSkill = new InstantSkill(data, skillType, primaryTargeting);

        return baseSkill;
    }
}

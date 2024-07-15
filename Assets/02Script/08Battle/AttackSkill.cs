using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    public class AttackSkill : MonoBehaviour, ISkill
    {
        // 타겟팅 + IDamageable에게 공격
        private IFindable findBehavior;

        public AttackSkill(IFindable findable)
        {
            findBehavior = findable;
        }

        public void UseSkill()
        {
            var target = findBehavior.FindTarget();
        }
    }

    public class BuffSkill : MonoBehaviour, ISkill
    {
        // 타겟팅 + 타겟의 status에 버프/디버프 제공
        private IFindable findBehavior;

        public BuffSkill(IFindable findable)
        {
            findBehavior = findable;
        }

        public void UseSkill()
        {
            throw new System.NotImplementedException();
        }
    }

    public class FieldSkill : MonoBehaviour, ISkill
    {
        // 스킬을 쓰는 장판 생성
        public void UseSkill()
        {
            throw new System.NotImplementedException();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이디어1. 버프를 줄 때, 따로 저장 -> 버프 리스트에 등록
// 아이디어2. 버프를 줄 때, 버프 리스트에 등록, 타이머를 최솟값으로 설정 -> 타이머를 0으로 설정

public class AreaSkill : MonoBehaviour
{
    private SkillData skillData;
    private float timer;

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
        // 1. other가 타겟인지
        // 2. otehr가 IDamageable을 갖고 있는지
    }

    private void OnTriggerExit(Collider other)
    {
    }
}

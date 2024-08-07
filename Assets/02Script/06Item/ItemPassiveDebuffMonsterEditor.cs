using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class ItemDebuffMonsterEditor<T> : Editor where T : ItemDebuffMonster
{
    public override void OnInspectorGUI()
    {
        T buffInfo = (T)target;

        if (buffInfo.IsPassive)
            EditorGUILayout.LabelField("패시브 스킬");
        else
            EditorGUILayout.LabelField("액티브 스킬");

        buffInfo.id = EditorGUILayout.IntField("ID", buffInfo.id);
        buffInfo.buffType = (BuffType)EditorGUILayout.EnumPopup("버프 타입", buffInfo.buffType);
        switch (buffInfo.buffType)
        {
            case BuffType.DOT_HP:
                buffInfo.damageTerm = EditorGUILayout.FloatField("도트 데미지 주기", buffInfo.damageTerm);
                break;
            case BuffType.NONE:
            case BuffType.MAX_HP:
            case BuffType.COUNT:
                EditorGUILayout.LabelField("해당 버프 타입은 사용이 불가합니다.");
                return;
        }
        buffInfo.isPercentage = EditorGUILayout.Toggle("퍼센트", buffInfo.isPercentage);
        buffInfo.buffValue = EditorGUILayout.FloatField("버프 값", buffInfo.buffValue);
        buffInfo.isPermanent = EditorGUILayout.Toggle("영구 적용", buffInfo.isPermanent);
        if (!buffInfo.isPermanent)
        {
            buffInfo.lastingTime = EditorGUILayout.FloatField("버프 지속 시간", buffInfo.lastingTime);
        }

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(target);
        }
    }
}

[CustomEditor(typeof(ItemPassiveDebuffMonster))]
public class ItemPassiveDebuffMonsterEditor : ItemDebuffMonsterEditor<ItemPassiveDebuffMonster> { }

[CustomEditor(typeof(ItemActiveDebuffMonster))]
public class ItemActiveDebuffMonsterEditor : ItemDebuffMonsterEditor<ItemActiveDebuffMonster> { }
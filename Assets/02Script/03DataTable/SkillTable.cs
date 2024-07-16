using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class SkillData
{
    public int Id { get; set; }
    public int Target { get; set; } // 플레이어, 몬스터
    public int RangeType { get; set; } // 단일, 범위, 장판
    public float RangeValue { get; set; } // 범위 스킬의 범위
    public float Damage { get; set; }
    public float CoolTime { get; set; }
    public float Duration { get; set; } // 설치 공격 지속 시간
    public float CastingTime { get; set; } // 플레이어의 애니메이션 지속 시간
    // public int IsDot { get; set; } // 도트 스킬
    public int BuffId { get; set; }
    public int AssetNo { get; set; }
}

public class SkillTable : DataTable
{
    public Dictionary<int, SkillData> table = new Dictionary<int, SkillData>();

    public SkillData Get(int id)
    {
        return table.GetValueOrDefault(id);
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        // var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<SkillData>();
            foreach (var record in records)
            {
                table.TryAdd(record.Id, record);
            }
        }
    }
}

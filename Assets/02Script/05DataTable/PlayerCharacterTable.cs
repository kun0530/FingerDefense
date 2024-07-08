using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class PlayerCharacterData
{
    public int Id { get; set; }
    public float Hp { get; set; }
    public int Grade { get; set; }
    public int Element { get; set; }
    public float AtkDmg { get; set; }
    public float AtkSpeed { get; set; }
    public float AtkRange { get; set; } // 공격 사정거리
    public int Skill { get; set; }
    public float CoolTime { get; set; } // 재소환 대기시간
    public int Priority { get; set; } // 소환된 캐릭터가 배치되는 자리. 이미 배치된 캐릭터가 있는 경우, 오름차순으로 배치
    public int AssetNo { get; set; }
}

public class PlayerCharacterTable : DataTable
{
    private Dictionary<int, PlayerCharacterData> table = new Dictionary<int, PlayerCharacterData>();

    public PlayerCharacterData Get(int id)
    {
        if (table.TryGetValue(id, out var value))
        {
            return value;
        }

        return null;
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<PlayerCharacterData>();
            foreach (var record in records)
            {
                if (!table.ContainsKey(record.Id))
                    table.Add(record.Id, record);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

// TO-DO: 추후 위치 변경해야 합니다.
public enum Elements
{
    SCISSOR,
    ROCK,
    PAPER
}

public class MonsterData
{
    public enum DragTypes
    {
        BOSS,
        NORMAL,
        SPECIAL
    }

    public int ID { get; set; }
    public float Hp{ get; set; }
    public int DragType { get; set; }
    public int Element { get; set; }
    public float MoveSpeed { get; set; }
    public float AtkDmg { get; set; }
    public float AtkSpeed { get; set; } // 초당 데미지
    public bool IsRanger { get; set; } // 원거리 타입 여부
    public float AtkRange { get; set; } // 원거리 몬스터 공격 거리
    public float Height { get; set; } // 즉사 높이
    public int Skill { get; set; }
    public int AssetNo { get; set; }

    public override string ToString()
    {
        return $"{ID}: {Hp} / {(DragTypes)DragType} / {Element}";
    }
}

public class MonsterTable : DataTable
{
    private Dictionary<int, MonsterData> table = new Dictionary<int, MonsterData>();

    public bool IsExist(int id)
    {
        return table.ContainsKey(id);
    }

    public MonsterData Get(int id)
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
            var records = csvReader.GetRecords<MonsterData>();
            foreach (var record in records)
            {
                if (!table.ContainsKey(record.ID))
                    table.Add(record.ID, record);
            }
        }
    }
}

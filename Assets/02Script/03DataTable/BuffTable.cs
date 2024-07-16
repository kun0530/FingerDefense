using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public enum BuffType
{
    ATK_SPEED,
    MOVE_SPEED,
    HP,
    ATK
}

public class BuffData
{
    public int Id { get; set; }
    public float LastingTime { get; set; }
    public float DmgTerm { get; set; }
    public int EffectNo { get; set; }
    public List<(int type, float value)> BuffActions = new();
}

public class BuffTable : DataTable
{
    public Dictionary<int, BuffData> table = new();

    public BuffData Get(int id)
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
            csvReader.Read();
            var columnCount = csvReader.ColumnCount;
            int patternStartIndex = 4;

            while (csvReader.Read())
            {
                var buffData = new BuffData()
                {
                    Id = csvReader.GetField<int>(0),
                    LastingTime = csvReader.GetField<int>(1),
                    DmgTerm = csvReader.GetField<float>(2),
                    EffectNo = csvReader.GetField<int>(3)
                };

                for (int i = patternStartIndex; i < columnCount; i += 2)
                {
                    (int type, float value) buff = (csvReader.GetField<int>(i), csvReader.GetField<int>(i + 1));
                    buffData.BuffActions.Add(buff);
                }

                if (!table.TryAdd(buffData.Id, buffData))
                    Logger.Log($"중복된 버프 아이디가 존재합니다!: {buffData.Id}");
            }
        }
    }
}

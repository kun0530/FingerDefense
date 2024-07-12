using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WaveData
{
    public int Stage { get; set; }
    public int Wave { get; set; }
    public float Term { get; set; }
    public List<(int monsterId, int monsterCount)> monsters = new List<(int monsterId, int monsterCount)>();
}

public class WaveTable : DataTable
{
    public Dictionary<(int stage, int wave), WaveData> table = new Dictionary<(int, int), WaveData>();

    public WaveData Get((int, int) id)
    {
        return table.GetValueOrDefault(id);
    }

    public WaveData Get(int stage, int wave)
    {
        return Get((stage, wave));
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        // var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
        var textAsset = Resources.Load<TextAsset>(path);
        var monsterTable = DataTableManager.Get<MonsterTable>(DataTableIds.Monster);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csvReader.Read();
            var columnCount = csvReader.ColumnCount;
            int patternStartIndex = 4;
            // int patternCount = (columnCount - patternStartIndex) / 2;

            while (csvReader.Read())
            {
                var waveData = new WaveData();
                waveData.Stage = csvReader.GetField<int>(1);
                waveData.Wave = csvReader.GetField<int>(2);
                waveData.Term = csvReader.GetField<float>(3);

                for (int i = patternStartIndex; i < columnCount; i += 2)
                {
                    (int monsterId, int monsterCount) monster = (csvReader.GetField<int>(i), csvReader.GetField<int>(i + 1));
                    if (!monsterTable.IsExist(monster.monsterId))
                    {
                        Logger.LogError($"존재하지 않는 몬스터 ID: {monster.monsterId}"); // 예외 던져야 함
                        continue;
                    }
                    waveData.monsters.Add(monster);
                }
                if (!table.ContainsKey((waveData.Stage, waveData.Wave)))
                    table.Add((waveData.Stage, waveData.Wave), waveData);
            }
        }

        // 무결성 검사
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CsvHelper;
using UnityEngine.AddressableAssets;

public class UpgradeData
{
    public int Id { get; set; }
    public int Name { get; set; }
    public int Type { get; set; }
    public int UpStatType { get; set; }
    public int Level { get; set; }
    public float UpStatValue { get; set; }
    public int UpgradePrice { get; set; }
    public int NeedClearStage { get; set; }
    public int PreUpgradeId { get; set; }
    public int UpgradeInfoId { get; set; }
    public int NeedCharId { get; set; }
    public int UpgradeResultId { get; set; }
    public int AssetNo { get; set; }
}

public class UpgradeTable : DataTable
{
    public Dictionary<int,UpgradeData> upgradeTable = new Dictionary<int, UpgradeData>();
    
    public UpgradeData Get(int id)
    {
        return upgradeTable.GetValueOrDefault(id);
    }
    
    public override void Load(string path)
    {
        path=string.Format(FormatPath, path);
        var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
        
        using var reader = new StringReader(textAsset.text);
        using var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        var records = csvReader.GetRecords<UpgradeData>();
        foreach (var record in records)
        {
            upgradeTable.TryAdd(record.Id, record);
        }
    }
}

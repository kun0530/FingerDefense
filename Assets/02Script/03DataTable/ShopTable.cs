using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;
using UnityEngine.AddressableAssets;

public class ShopData
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public int DescId { get; set; }
    public int PayType { get; set; }
    public int Price { get; set; }
    public int ItemId { get; set; }
    public int Amount { get; set; }
    public int AssetNo { get; set; }
}

public class ShopTable : DataTable
{
    public Dictionary<int,ShopData>table=new Dictionary<int, ShopData>();
    
    public bool IsExist(int id)
    {
        return table.ContainsKey(id);
    }
    
    public ShopData Get(int id)
    {
        return table.GetValueOrDefault(id);
    }
    
    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);
        var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
        using var reader = new StringReader(textAsset.text);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<ShopData>();
        foreach (var record in records)
        {
            table.Add(record.Id, record);
        }
    }
}



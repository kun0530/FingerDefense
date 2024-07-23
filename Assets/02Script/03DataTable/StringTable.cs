using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.IO;
using System.Text;

public class StringData
{
    public int Id { get; set; }
    public string Text { get; set; }
}


public class StringTable : DataTable
{
    private readonly Dictionary<int, string> table = new Dictionary<int, string>();

    public string Get(int id)
    {
        return table.GetValueOrDefault(id);
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();
        //var textAsset = Resources.Load<TextAsset>(path);

        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(textAsset.text)), Encoding.UTF8);
        using var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
        var records = csvReader.GetRecords<StringData>();
        foreach (var record in records)
        {
            table.TryAdd(record.Id, record.Text);
        }
    }
}

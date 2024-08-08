using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CsvHelper;
using System.IO;
using System.Text;

public class StringData
{
    public string Id { get; set; }
    public string Text { get; set; }
}


public class StringTable : DataTable
{
    private readonly Dictionary<string, string> table = new Dictionary<string, string>();
    

    public string Get(string id)
    {
        return table.GetValueOrDefault(id);
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Addressables.LoadAssetAsync<TextAsset>(path).WaitForCompletion();

        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(textAsset.text)), Encoding.UTF8);
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csvReader.GetRecords<StringData>();
        foreach (var record in records)
        {
            string processedText = record.Text.Replace("\\n", "\n");
            table.TryAdd(record.Id, processedText);  // 변경: int에서 string으로 변경
        }
    }

   
    
}

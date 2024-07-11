using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableIds
{
    public static readonly string Monster = "MonsterTable";
    public static readonly string Wave = "WaveTable";
    public static readonly string PlayerCharacter = "PlayerCharacterTable";
}

public static class DataTableManager
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        MonsterTable monsterTable = new MonsterTable();
        monsterTable.Load(DataTableIds.Monster);
        tables.Add(DataTableIds.Monster, monsterTable);

        WaveTable waveTable = new WaveTable();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);

        PlayerCharacterTable playerCharacterTable = new PlayerCharacterTable();
        playerCharacterTable.Load(DataTableIds.PlayerCharacter);
        tables.Add(DataTableIds.PlayerCharacter, playerCharacterTable);
    }

    // public static StringTable GetStringTable()
    // {
    //     return Get<StringTable>(DataTableIds.String[(int)Variables.SaveData.CurrentLang]);
    // }

    public static T Get<T>(string id) where T : DataTable
    {
        if (tables.TryGetValue(id, out var value))
        {
            return value as T;
        }
        return null;
    }
}
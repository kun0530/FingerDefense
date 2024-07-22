using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableIds
{
    public static readonly string Monster = "MonsterTable";
    public static readonly string Wave = "WaveTable";
    public static readonly string PlayerCharacter = "CharacterTable";
    public static readonly string Skill = "SkillTable";
    public static readonly string Buff = "BuffDebuffTable";
    public static readonly string Stage = "StageTable";
    public static readonly string Asset = "AssetListTable";

}

public static class DataTableManager
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        MonsterTable monsterTable = new();
        monsterTable.Load(DataTableIds.Monster);
        tables.Add(DataTableIds.Monster, monsterTable);

        WaveTable waveTable = new();
        waveTable.Load(DataTableIds.Wave);
        tables.Add(DataTableIds.Wave, waveTable);

        PlayerCharacterTable playerCharacterTable = new();
        playerCharacterTable.Load(DataTableIds.PlayerCharacter);
        tables.Add(DataTableIds.PlayerCharacter, playerCharacterTable);
        
        SkillTable skillTable = new();
        skillTable.Load(DataTableIds.Skill);
        tables.Add(DataTableIds.Skill, skillTable);

        BuffTable buffTable = new();
        buffTable.Load(DataTableIds.Buff);
        tables.Add(DataTableIds.Buff, buffTable);
        
        StageTable stageTable = new StageTable();
        stageTable.Load(DataTableIds.Stage);
        tables.Add(DataTableIds.Stage, stageTable);

        AssetListTable assetList = new AssetListTable();
        assetList.Load(DataTableIds.Asset);
        tables.Add(DataTableIds.Asset, assetList);
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

        Logger.LogError($"해당 데이터 테이블의 내용이 없습니다: {id}");
        return null;
    }
}
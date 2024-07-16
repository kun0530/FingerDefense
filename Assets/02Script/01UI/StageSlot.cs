using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CsvHelper;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StageSlot : MonoBehaviour
{
    public int stageNum;
    public Image[] MonsterImage;
    public TextMeshProUGUI stageName;
    
    public PlayerCharacterData playerCharacterData;
    
    private void Start()
    {
        LoadData();
    }
    public void LoadData()
    {
        // Addressables.LoadAssetAsync<PlayerCharacterData>("Assets/02Script/03DataTable/PlayerCharacterData.csv").Completed += handle =>
        // {
        //     if (handle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         playerCharacterData = handle.Result;
        //     }
        //     else
        //     {
        //         Debug.LogError("Failed to load data.");
        //     }
        // };
    }
    public void SetSlot(int stageNum)
    {
        
        
    }
    
    public void OnClickSlot()
    {
        //덱 편성 UI 호출
        
    }
}

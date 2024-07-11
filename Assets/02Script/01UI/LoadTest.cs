using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadTest : MonoBehaviour
{
    public Canvas loadCanvas;


    private void LoadPrefab()
    {
        Addressables.InstantiateAsync("Assets/03Prefab/01UI/StageUI.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 프리팹을 로드하고 loadCanvas의 자식으로 설정
                var instance = handle.Result;
                instance.transform.SetParent(loadCanvas.transform, false);
                
            }
            else
            {
                Debug.LogError("Failed to load prefab.");
            }
        };
        
        
        
    }

    void Start()
    {
        LoadPrefab();
    }

}

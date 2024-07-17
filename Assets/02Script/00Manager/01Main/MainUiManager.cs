using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MainUiManager : MonoBehaviour
{
    public Canvas loadCanvas;
    public Button stageButton;
    
    void Start()
    {
        LoadPrefab();
    }

    private void LoadPrefab()
    {
        Addressables.InstantiateAsync("Assets/02Script/03Prefab/01UI/StageUI.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 프리팹을 로드하고 loadCanvas의 자식으로 설정
                var instance = handle.Result;
                instance.transform.SetParent(loadCanvas.transform, false);
                instance.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to load prefab.");
            }
        };
        
        Addressables.InstantiateAsync("Assets/02Script/03Prefab/01UI/00MainScene/Deck Panel.prefab").Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // 프리팹을 로드하고 loadCanvas의 자식으로 설정
                var instance = handle.Result;
                instance.transform.SetParent(loadCanvas.transform, false);
                instance.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Failed to load prefab.");
            }
        };
    }
    
    
}

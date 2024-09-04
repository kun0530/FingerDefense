using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class RemoteDownLoad : MonoBehaviour
{
    public Button startButton;

    private List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();

    private void Start()
    {
        startButton.onClick.AddListener(LoadGameSceneWrapper);

        // 게임 시작 전에 필요한 모든 어드레서블 에셋들을 미리 로드합니다.
        PreloadAllAddressableAssets().Forget();
    }

    private async UniTaskVoid PreloadAllAddressableAssets()
    {
        try
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync("Default");
            await locationsHandle;

            if (locationsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var location in locationsHandle.Result)
                {
                    var handle = Addressables.DownloadDependenciesAsync(location);
                    handles.Add(handle);
                    await handle;
                }

                Logger.Log("All assets preloaded.");
            }
            else
            {
                Logger.LogError("Failed to load resource locations.");
            }
        }
        catch (Exception e)
        {
            Logger.LogError($"Preload failed: {e.Message}");
        }
    }

    private void LoadGameSceneWrapper()
    {
        // 버튼 클릭 시, 미리 로드된 에셋을 해제하고 씬을 전환합니다.
        UnloadAllAddressableAssets();
        LoadGameScene();
    }

    private void UnloadAllAddressableAssets()
    {
        foreach (var handle in handles)
        {
            Addressables.Release(handle);
        }
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1); // 씬 전환
    }
}
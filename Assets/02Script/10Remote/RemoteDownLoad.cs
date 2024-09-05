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
        if (GameManager.instance.GameData.NicknameCheck)
        {
            UnloadAllAddressableAssets();
            LoadGameScene();
            return;
        }

        ModalWindow.Create(window =>
        {
            window.SetHeader("스킵 확인")
                .SetBody("튜토리얼을 스킵하시겠습니까?")
                .AddButton("확인", () =>
                {
                    SkipTutorial();
                    UnloadAllAddressableAssets();
                    LoadGameScene();
                })
                .AddButton("취소", () =>
                {
                    UnloadAllAddressableAssets();
                    LoadGameScene();
                })
                .Show();
        });
    }

    // 버튼 클릭 시, 미리 로드된 에셋을 해제하고 씬을 전환합니다.
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

    private void SkipTutorial()
    {
        var saveData = GameManager.instance.GameData;
        saveData.PlayerName = "Skip";

        saveData.ObtainedGachaIDs.Add(300470);
        saveData.ObtainedGachaIDs.Add(300596);
        saveData.ObtainedGachaIDs.Add(300530);
        saveData.characterIds.Add(300470);
        saveData.characterIds.Add(300596);
        saveData.characterIds.Add(300530);

        saveData.Gold = 1000;
        saveData.Diamond = 1000;
        saveData.Ticket = 10;

        saveData.NicknameCheck = true;
        saveData.StageChoiceTutorialCheck = true;
        saveData.DeckUITutorialCheck = true;
        saveData.Game1TutorialCheck = true;
        saveData.Game2TutorialCheck = true;
        saveData.Game3TutorialCheck = true;
        saveData.ShopDragTutorialCheck = true;
        saveData.ShopGimmickTutorialCheck = true;
        saveData.ShopCharacterTutorialCheck = true;
        saveData.ShopFeatureTutorialCheck = true;
    }
}
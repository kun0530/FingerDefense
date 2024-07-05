using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DownLoadManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject waitMessage;
    public GameObject downloadMessage;
    public Slider downSlider;
    public TextMeshProUGUI sizeInfoText;
    public TextMeshProUGUI downValText;

    [Header("Label")]
    public AssetLabelReference defaultLabel;

    private long patchSize;
    private Dictionary<string, int> patchMap = new Dictionary<string, int>();

    private void Start()
    {
        waitMessage.SetActive(true);
        downloadMessage.SetActive(false);

        InitAddressable().Forget();
        CheckUpdateFiles().Forget();
    }

    private async UniTask InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        await init;
    }

    #region UpdateCheck
    private async UniTask CheckUpdateFiles()
    {
        var labels = new List<string>() { defaultLabel.labelString };
        patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;

            patchSize += handle.Result;
        }

        if (patchSize > 0)
        {
            waitMessage.SetActive(false);
            downloadMessage.SetActive(true);
            sizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            downSlider.gameObject.SetActive(false);
            sizeInfoText.text = "게임에 접속중...";
            await UniTask.Delay(2000);
            LoadGameScene();
        }
    }

    private string GetFileSize(long byteCnt)
    {
        var size = "0 bytes";
        if (byteCnt >= 1073741824.0)
            size = $"{byteCnt / 1073741824.0:##.##}" + " GB";
        else if (byteCnt >= 1048576.0)
            size = $"{byteCnt / 1048576.0:##.##}" + " MB";
        else if (byteCnt >= 1024.0)
            size = $"{byteCnt / 1024.0:##.##}" + " KB";
        else if (byteCnt > 0 && byteCnt < 1024.0)
            size = byteCnt.ToString() + " bytes";

        return size;
    }
    #endregion

    public void ButtonDownload()
    {
        PatchFiles().Forget();
    }

    private async UniTask PatchFiles()
    {
        var labels = new List<string>() { defaultLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);
            await handle;

            if (handle.Result != 0)
            {
                Download(label).Forget();
            }
        }

        await CheckDownload();
    }

    private async Task CheckDownload()
    {
        var total = 0;
        sizeInfoText.text = "0%";

        while (true)
        {
            total += patchMap.Sum(tmp => tmp.Value);
            downSlider.value = (float)total / patchSize;
            sizeInfoText.text = $"{downSlider.value * 100:##}%";

            if (total == patchSize)
            {
                LoadGameScene();
                break;
            }
            total = 0;
            await UniTask.DelayFrame(1);
        }
    }

    private async UniTaskVoid Download(string label)
    {
        patchMap.Add(label, 0);
        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = (int)handle.GetDownloadStatus().DownloadedBytes;
            await UniTask.DelayFrame(1);
        }

        patchMap[label] = (int)handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    private void LoadGameScene()
    {
        // 게임 씬으로 전환
        LoadingManager.LoadScene("Test");
    }
}

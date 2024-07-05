using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;


public class DownLoadManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject waitMessage;
    public GameObject downloadMessage;
    public Slider downSlider;
    public TextMeshProUGUI sizeInfoText;
    public TextMeshProUGUI downValText;
    public Button LoadingButton;

     
    [Header("Label")]
    public AssetLabelReference defaultLabel;

    private long patchSize;
    private Dictionary<string, int> patchMap = new Dictionary<string, int>();

    
    //Release 해줄것 
    private void Start()
    {
        waitMessage.SetActive(true);
        downloadMessage.SetActive(false);
        LoadingButton.interactable = false;
        LoadingButton.onClick.AddListener(LoadGameScene); // 버튼 클릭 이벤트 추가
        ButtonDownload(); // 다운로드 시작
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
            downValText.text = GetFileSize(patchSize);
            sizeInfoText.text = "업데이트 용량을 확인하고 있습니다.";
        }
        else
        {
            downSlider.gameObject.SetActive(false);
            sizeInfoText.text = "화면을 터치해 게임을 시작해주세요";
            LoadingButton.interactable = true;
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
                sizeInfoText.text = "다운로드 완료";
                LoadingButton.interactable = true;
                break;
            }
            total = 0;
            await UniTask.DelayFrame(1);
        }
    }

    private async UniTaskVoid Download(string label)
    {
        patchMap.TryAdd(label, 0);

        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = (int)handle.GetDownloadStatus().DownloadedBytes;
            await UniTask.DelayFrame(1);
        }

        patchMap[label] = (int)handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    public void LoadGameScene()
    {
        LoadingManager.LoadScene("Test");
    }
}

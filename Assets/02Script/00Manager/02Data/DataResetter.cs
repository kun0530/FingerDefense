using UnityEngine;
using System.IO;

public class DataResetter : MonoBehaviour
{
    [Header("파일 저장 방식의 데이터를 삭제하려면 체크하세요.")]
    public bool deleteSavedFiles = true;

    [Header("PlayerPrefs 데이터를 삭제하려면 체크하세요.")]
    public bool deletePlayerPrefs = true;
    
    [Header("테스트 모드에서 재화를 증가시키려면 체크하세요.")]
    public bool increaseTestResources = true;
    
    // 데이터 파일들의 이름 리스트
    private string[] dataFiles = {
        "GameData.json", // 예시 파일 이름, 실제 저장 파일 이름을 사용하세요
        // 추가적인 파일 이름들을 여기에 추가하세요
    };
    public int goldIncreaseAmount = 1000;
    public int gemIncreaseAmount = 1000;
    public int ticketIncreaseAmount = 5;
    public int mileageIncreaseAmount = 100;
    
    private GameManager gameManager;
    
    void Start()
    {
        gameManager = GameManager.instance;
        if (deletePlayerPrefs)
        {
            ResetPlayerPrefs();
        }

        if (deleteSavedFiles)
        {
            DeleteSavedFiles();
        }

        if (increaseTestResources || Input.GetKey(KeyCode.F1))
        {
            IncreaseTestResources();
        }
    }

    private void IncreaseTestResources()
    {
        if(gameManager != null && gameManager.GameData != null)
        {
            gameManager.GameData.Gold += goldIncreaseAmount;
            gameManager.GameData.Diamond += gemIncreaseAmount;
            gameManager.GameData.Ticket += ticketIncreaseAmount;
            gameManager.GameData.Mileage += mileageIncreaseAmount;
            
            Debug.Log("테스트 모드에서 재화를 증가시켰습니다.");
            DataManager.SaveFile(gameManager.GameData);
        }
    }

    // PlayerPrefs 초기화
    private void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs 데이터를 초기화했습니다.");
    }

    // 저장된 파일들 삭제
    private void DeleteSavedFiles()
    {
        foreach (var fileName in dataFiles)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"{fileName} 파일을 삭제했습니다.");
            }
            else
            {
                Debug.Log($"{fileName} 파일을 찾을 수 없습니다.");
            }
        }
    }
}
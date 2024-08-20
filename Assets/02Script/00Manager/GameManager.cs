using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameData GameData { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;

        DataManager.LoadEncryptionKeyAndIv();
        GameData = DataManager.LoadFile();
        if (GameData == null)
        {
            GameData = new GameData();
            GameData.Init();
        }
    }
}
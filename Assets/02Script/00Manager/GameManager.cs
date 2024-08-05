using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    private DataManager dataManager;

    private string playerName;
    private bool nicknameCheck;
    private bool stageChoiceTutorialCheck;
    private bool deckUITutorialCheck;
    private bool game1TutorialCheck;
    private bool game2TutorialCheck;
    private bool game3TutorialCheck;
    private bool game4TutorialCheck;
    
    [NotNull]
    public static GameManager instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<GameManager>();
                if (Instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    Instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return Instance;
        }
        private set => Instance = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;

        dataManager = GetComponent<DataManager>();
    }

    private void Start()
    {
        LoadGameData();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGameData();
            Logger.Log("Game data has been reset.");
        }       
#endif
    }

    private void LoadGameData()
    {
        GameData gameData = dataManager.LoadFile<GameData>("GameData.json") ?? new GameData();

        playerName = gameData.PlayerName;
        nicknameCheck = gameData.NicknameCheck;
        stageChoiceTutorialCheck = gameData.StageChoiceTutorialCheck;
        deckUITutorialCheck = gameData.DeckUITutorialCheck;
        game1TutorialCheck = gameData.Game1TutorialCheck;
        game2TutorialCheck = gameData.Game2TutorialCheck;
        game3TutorialCheck = gameData.Game3TutorialCheck;
        game4TutorialCheck = gameData.Game4TutorialCheck;
    }

    private void SaveGameData()
    {
        GameData gameData = new GameData
        {
            PlayerName = playerName,
            NicknameCheck = nicknameCheck,
            StageChoiceTutorialCheck = stageChoiceTutorialCheck,
            DeckUITutorialCheck = deckUITutorialCheck,
            Game1TutorialCheck = game1TutorialCheck,
            Game2TutorialCheck = game2TutorialCheck,
            Game3TutorialCheck = game3TutorialCheck,
            Game4TutorialCheck = game4TutorialCheck
        };
        dataManager.SaveFile("GameData.json", gameData);
    }
    
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            SaveGameData();
        }
    }

    public bool NicknameCheck
    {
        get => nicknameCheck;
        set
        {
            nicknameCheck = value;
            SaveGameData();
        }
    }

    public bool StageChoiceTutorialCheck
    {
        get => stageChoiceTutorialCheck;
        set
        {
            stageChoiceTutorialCheck = value;
            SaveGameData();
        }
    }

    public bool DeckUITutorialCheck
    {
        get => deckUITutorialCheck;
        set
        {
            deckUITutorialCheck = value;
            SaveGameData();
        }
    }

    public bool Game1TutorialCheck
    {
        get => game1TutorialCheck;
        set
        {
            game1TutorialCheck = value;
            SaveGameData();
        }
    }
    
    public bool Game2TutorialCheck
    {
        get => game2TutorialCheck;
        set
        {
            game2TutorialCheck = value;
            SaveGameData();
        }
    }
    
    public bool Game3TutorialCheck
    {
        get => game3TutorialCheck;
        set
        {
            game3TutorialCheck = value;
            SaveGameData();
        }
    }
    
    public bool Game4TutorialCheck
    {
        get => game4TutorialCheck;
        set
        {
            game4TutorialCheck = value;
            SaveGameData();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainScene":
                break;
            case "Game Scene":
                break;
        }
    }

    //테스트용 코드 삭제 예정
    private void ResetGameData()
    {
        // 데이터 초기화
        playerName = "";
        nicknameCheck = false;
        stageChoiceTutorialCheck = false;
        deckUITutorialCheck = false;
        game1TutorialCheck = false;
        game2TutorialCheck = false;
        game3TutorialCheck = false;
        game4TutorialCheck = false;
        
        SceneManager.LoadScene(0);
        // 초기화된 데이터 저장
        SaveGameData();
    }

}

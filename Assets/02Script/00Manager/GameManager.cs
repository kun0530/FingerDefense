using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    private DataManager dataManager;

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

        _playerName = gameData.PlayerName;
        _nicknameCheck = gameData.NicknameCheck;
        _stageChoiceTutorialCheck = gameData.StageChoiceTutorialCheck;
        _deckUITutorialCheck = gameData.DeckUITutorialCheck;
        _gameTutorialCheck = gameData.GameTutorialCheck;
    }

    private void SaveGameData()
    {
        GameData gameData = new GameData
        {
            PlayerName = _playerName,
            NicknameCheck = _nicknameCheck,
            StageChoiceTutorialCheck = _stageChoiceTutorialCheck,
            DeckUITutorialCheck = _deckUITutorialCheck,
            GameTutorialCheck = _gameTutorialCheck
        };
        dataManager.SaveFile("GameData.json", gameData);
    }

    private string _playerName;
    private bool _nicknameCheck;
    private bool _stageChoiceTutorialCheck;
    private bool _deckUITutorialCheck;
    private bool _gameTutorialCheck;

    public string PlayerName
    {
        get => _playerName;
        set
        {
            _playerName = value;
            SaveGameData();
        }
    }

    public bool NicknameCheck
    {
        get => _nicknameCheck;
        set
        {
            _nicknameCheck = value;
            SaveGameData();
        }
    }

    public bool StageChoiceTutorialCheck
    {
        get => _stageChoiceTutorialCheck;
        set
        {
            _stageChoiceTutorialCheck = value;
            SaveGameData();
        }
    }

    public bool DeckUITutorialCheck
    {
        get => _deckUITutorialCheck;
        set
        {
            _deckUITutorialCheck = value;
            SaveGameData();
        }
    }

    public bool GameTutorialCheck
    {
        get => _gameTutorialCheck;
        set
        {
            _gameTutorialCheck = value;
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
        _playerName = "";
        _nicknameCheck = false;
        _stageChoiceTutorialCheck = false;
        _deckUITutorialCheck = false;
        _gameTutorialCheck = false;

        // 초기화된 데이터 저장
        SaveGameData();
    }

}

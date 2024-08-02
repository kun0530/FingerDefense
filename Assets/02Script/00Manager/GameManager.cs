using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    
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
        LoadPlayerName();
        
        Application.targetFrameRate = 60;
    }

    private void LoadPlayerName()
    {
        Variables.LoadName.Nickname = PlayerName;
    }

    public string PlayerName
    {
        get => PlayerPrefs.GetString("PlayerName", "");
        set
        {
            PlayerPrefs.SetString("PlayerName", value);
            PlayerPrefs.Save();
        }
    }
    public bool NicknameCheck
    {
        get => PlayerPrefs.GetInt("NicknameCheck", 0) == 1;
        set => PlayerPrefs.SetInt("NicknameCheck", value ? 1 : 0);
    }
    
    public bool StageChoiceTutorialCheck
    {
        get => PlayerPrefs.GetInt("StageChoiceTutorialCheck", 0) == 1;
        set => PlayerPrefs.SetInt("StageChoiceTutorialCheck", value ? 1 : 0);
    }

    public bool DeckUITutorialCheck
    {
        get => PlayerPrefs.GetInt("DeckUITutorialCheck", 0) == 1;
        set => PlayerPrefs.SetInt("DeckUITutorialCheck", value ? 1 : 0);
    }

    public bool GameTutorialCheck
    {
        get => PlayerPrefs.GetInt("GameTutorialCheck", 0) == 1;
        set => PlayerPrefs.SetInt("GameTutorialCheck", value ? 1 : 0);
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
    
    
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//TO-DO Prototype 이후 삭제 예정
public class GameManager : MonoBehaviour
{
    //public GameUiManager GameUiManager { get; private set; }
    //public MainUiManager MainUiManager { get; private set; }

    public GameTutorialManager GameTutorial; 
    public MonsterSpawner monsterSpawner;
    
    
    //To-Do : 프로토타입 용 계속 뜨는거 방지용 변수, 뒤에 Json으로 옮기고 삭제 예정
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
    
    private void Awake()
    {
        
    }
    
    private void Start()
    {
        if (GameTutorial == null)
        {
            Logger.Log("GameTutorialManager is null, 삭제 예정");
        }
        if (monsterSpawner == null)
        {
            Logger.Log("MonsterSpawner is null, 삭제 예정");
        }
        
        if(GameTutorial && !GameTutorialCheck)
        {
            GameTutorial.StartTutorial(() =>
            {
                Logger.Log("게임 튜토리얼 시작");
            });
        }

        if (GameTutorial && GameTutorialCheck)
        {
            monsterSpawner.isTutorialCompleted = true;    
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

    
}

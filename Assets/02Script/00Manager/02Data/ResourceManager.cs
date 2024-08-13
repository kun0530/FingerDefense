using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager, IResourceSubject
{
    private IDataManager dataManager;

    // 플레이어 정보 및 튜토리얼 체크
    private string playerName;
    private bool nicknameCheck;
    private bool stageChoiceTutorialCheck;
    private bool deckUITutorialCheck;
    private bool game1TutorialCheck;
    private bool game2TutorialCheck;
    private bool game3TutorialCheck;
    private bool game4TutorialCheck;

    // 리소스 관리
    private int gold;
    private int diamond;
    private int ticket;
    private int mileage;
    private List<int> obtainedGachaIDs = new List<int>();
    private List<(int itemId, int itemCount)> items = new List<(int, int)>();
    public List<(int MonsterGimmick, int level)> MonsterGimmickLevel = new List<(int, int)>();
    public List<(int PlayerUpgrade, int level)> PlayerUpgradeLevel = new List<(int, int)>();
    private List<IResourceObserver> observers = new List<IResourceObserver>();

    private void Awake()
    {
        dataManager = DataManager.Instance;
        LoadData();
    }

    // 플레이어 정보 및 튜토리얼 관련 프로퍼티
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            SaveData();
        }
    }

    public bool NicknameCheck
    {
        get => nicknameCheck;
        set
        {
            nicknameCheck = value;
            NotifyObservers(ResourceType.NicknameCheck, nicknameCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool StageChoiceTutorialCheck
    {
        get => stageChoiceTutorialCheck;
        set
        {
            stageChoiceTutorialCheck = value;
            NotifyObservers(ResourceType.StageChoiceTutorialCheck, stageChoiceTutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool DeckUITutorialCheck
    {
        get => deckUITutorialCheck;
        set
        {
            deckUITutorialCheck = value;
            NotifyObservers(ResourceType.DeckUITutorialCheck, deckUITutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool Game1TutorialCheck
    {
        get => game1TutorialCheck;
        set
        {
            game1TutorialCheck = value;
            NotifyObservers(ResourceType.Game1TutorialCheck, game1TutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool Game2TutorialCheck
    {
        get => game2TutorialCheck;
        set
        {
            game2TutorialCheck = value;
            NotifyObservers(ResourceType.Game2TutorialCheck, game2TutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool Game3TutorialCheck
    {
        get => game3TutorialCheck;
        set
        {
            game3TutorialCheck = value;
            NotifyObservers(ResourceType.Game3TutorialCheck, game3TutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    public bool Game4TutorialCheck
    {
        get => game4TutorialCheck;
        set
        {
            game4TutorialCheck = value;
            NotifyObservers(ResourceType.Game4TutorialCheck, game4TutorialCheck ? 1 : 0);
            SaveData();
        }
    }

    // 리소스 관리 프로퍼티
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            NotifyObservers(ResourceType.Gold, gold);
            SaveData();
        }
    }

    public int Diamond
    {
        get => diamond;
        set
        {
            diamond = value;
            NotifyObservers(ResourceType.Diamond, diamond);
            SaveData();
        }
    }

    public int Ticket
    {
        get => ticket;
        set
        {
            ticket = value;
            NotifyObservers(ResourceType.Ticket, ticket);
            SaveData();
        }
    }

    public int Mileage
    {
        get => mileage;
        set
        {
            mileage = value;
            NotifyObservers(ResourceType.Mileage, mileage);
            SaveData();
        }
    }

    public List<int> ObtainedGachaIDs
    {
        get => obtainedGachaIDs;
        set
        {
            obtainedGachaIDs = value;
            SaveData();
        }
    }

    public List<(int itemId, int itemCount)> Items
    {
        get => items;
        set
        {
            items = value;
            SaveData();
        }
    }
    
    public List<(int MonsterGimmick, int level)> monsterGimmickLevel
    {
        get => MonsterGimmickLevel;
        set
        {
            MonsterGimmickLevel = value;
            SaveData();
        }
    }
    
    public List<(int PlayerUpgrade, int level)> playerUpgradeLevel
    {
        get => PlayerUpgradeLevel;
        set
        {
            PlayerUpgradeLevel = value;
            SaveData();
        }
    }
    
    public void AddItem(int itemId, int itemCount)
    {
        var itemIndex = Items.FindIndex(i => i.itemId == itemId);
        if (itemIndex != -1)
        {
            Items[itemIndex] = (itemId, Items[itemIndex].itemCount + itemCount);
        }
        else
        {
            Items.Add((itemId, itemCount));
        }
        NotifyObservers(ResourceType.ItemCount, itemCount);
        SaveData();
    }
    
    public void RemoveItem(int itemId, int itemCount)
    {
        var itemIndex = Items.FindIndex(i => i.itemId == itemId);
        if (itemIndex != -1)
        {
            Items[itemIndex] = (itemId, Items[itemIndex].itemCount - itemCount);
        }
        else
        {
            Items.Add((itemId, itemCount));
        }
        NotifyObservers(ResourceType.ItemCount, itemCount);
        SaveData();
    }

    public void SaveData()
    {
        var gameData = new GameData
        {
            PlayerName = playerName,
            NicknameCheck = nicknameCheck,
            StageChoiceTutorialCheck = stageChoiceTutorialCheck,
            DeckUITutorialCheck = deckUITutorialCheck,
            Game1TutorialCheck = game1TutorialCheck,
            Game2TutorialCheck = game2TutorialCheck,
            Game3TutorialCheck = game3TutorialCheck,
            Game4TutorialCheck = game4TutorialCheck,
            Gold = gold,
            Diamond = diamond,
            Ticket = ticket,
            Mileage = mileage,
            ObtainedGachaIDs = obtainedGachaIDs,
            ItemId = new List<(int, int)>(items)
            
        };
        dataManager.SaveFile("GameData.json", gameData);
    }

    public void LoadData()
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
        gold = gameData.Gold;
        diamond = gameData.Diamond;
        ticket = gameData.Ticket;
        mileage = gameData.Mileage;
        obtainedGachaIDs = gameData.ObtainedGachaIDs;
        items = gameData.ItemId;
    }

    // 옵저버 패턴 관련 메서드
    public void RegisterObserver(IResourceObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(IResourceObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public void NotifyObservers(ResourceType resourceType, int newValue)
    {
        foreach (var observer in observers)
        {
            observer.OnResourceUpdate(resourceType, newValue);
        }
    }
}

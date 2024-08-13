using System;
using System.Collections.Generic;

[Serializable]
public class GameData : IResourceSubject
{
    public string PlayerName = "";
    private int gold;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            NotifyObservers(ResourceType.Gold, gold);
        }
    }
    private int diamond;
    public int Diamond
    {
        get => diamond;
        set
        {
            diamond = value;
            NotifyObservers(ResourceType.Diamond, diamond);
        }
    }
    private int ticket;
    public int Ticket
    {
        get => ticket;
        set
        {
            ticket = value;
            NotifyObservers(ResourceType.Ticket, ticket);
        }
    }
    private int mileage;
    public int Mileage
    {
        get => mileage;
        set
        {
            mileage = value;
            NotifyObservers(ResourceType.Mileage, mileage);
        }
    }
    private bool nicknameCheck;
    public bool NicknameCheck
    {
        get => NicknameCheck;
        set
        {
            nicknameCheck = value;
            NotifyObservers(ResourceType.NicknameCheck, nicknameCheck ? 1 : 0);
        }
    }
    private bool stageChoiceTutorialCheck;
    public bool StageChoiceTutorialCheck
    {
        get => stageChoiceTutorialCheck;
        set
        {
            stageChoiceTutorialCheck = value;
            NotifyObservers(ResourceType.StageChoiceTutorialCheck, stageChoiceTutorialCheck ? 1 : 0);
        }
    }
    private bool deckUITutorialCheck;
    public bool DeckUITutorialCheck
    {
        get => deckUITutorialCheck;
        set
        {
            deckUITutorialCheck = value;
            NotifyObservers(ResourceType.DeckUITutorialCheck, deckUITutorialCheck ? 1 : 0);
        }
    }
    private bool game1TutorialCheck;
    public bool Game1TutorialCheck
    {
        get => game1TutorialCheck;
        set
        {
            game1TutorialCheck = value;
            NotifyObservers(ResourceType.Game1TutorialCheck, game1TutorialCheck ? 1 : 0);
        }
    }
    private bool game2TutorialCheck;
    public bool Game2TutorialCheck
    {
        get => game2TutorialCheck;
        set
        {
            game2TutorialCheck = value;
            NotifyObservers(ResourceType.Game2TutorialCheck, game2TutorialCheck ? 1 : 0);
        }
    }
    private bool game3TutorialCheck;
    public bool Game3TutorialCheck
    {
        get => game3TutorialCheck;
        set
        {
            game3TutorialCheck = value;
            NotifyObservers(ResourceType.Game3TutorialCheck, game3TutorialCheck ? 1 : 0);
        }
    }
    private bool game4TutorialCheck;
    public bool Game4TutorialCheck
    {
        get => game4TutorialCheck;
        set
        {
            game4TutorialCheck = value;
            NotifyObservers(ResourceType.Game4TutorialCheck, game4TutorialCheck ? 1 : 0);
        }
    }
    
    public HashSet<int> MonsterDragIds = new HashSet<int>();
    
    public List<int> ObtainedGachaIDs = new List<int>();
    public List<(int itemId, int itemCount)> Items = new List<(int, int)>();
    
    public List<(int monsterGimmick, int level)> MonsterGimmickLevel = new List<(int, int)>();
    public List<(int playerUpgrade, int level)> PlayerUpgradeLevel = new List<(int, int)>();
    
    public int StageClearCount; 
    enum TutorialCheck
    {
        NONE =-1,
        NICK = 0,
        STAGE,
        DECK,
        GAME1,
        GAME2,
        GAME3,
        GAME4
    }

    enum MonsterGimmick
    {
        NONE = -1,
        ATTACKRANGE=0,
        ATTACKDAMAG=1,
        ATTACKDURATION=2,
    }
    MonsterGimmick monsterGimmick;
    enum PlayerUpgrade
    {
        NONE = -1,
        CHARACTER_ARRANGEMENT=0,
        PLAYER_HEALTH=1,
        INCREASE_DRAG=2,
    }
    PlayerUpgrade playerUpgrade;

    public void AddItem(int itemId, int itemCount)
    {
        var existingItem = Items.Find(item => item.itemId == itemId);
        if (existingItem != (0, 0))
        {
            existingItem.itemCount += itemCount;
        }
        else
        {
            Items.Add((itemId, itemCount));
        }
    }
    
    public void RemoveItem(int itemId, int itemCount)
    {
        var existingItem = Items.Find(item => item.itemId == itemId);
        if (existingItem != (0, 0))
        {
            existingItem.itemCount -= itemCount;
            if (existingItem.itemCount <= 0)
            {
                Items.Remove(existingItem);
            }
        }
    }

    private List<IResourceObserver> observers = new List<IResourceObserver>();

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
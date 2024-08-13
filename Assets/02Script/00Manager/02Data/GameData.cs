using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public string PlayerName = "";
    public int Gold;
    public int Diamond;
    public int Ticket;
    public int Mileage;
    public bool NicknameCheck;
    public bool StageChoiceTutorialCheck;
    public bool DeckUITutorialCheck;
    public bool Game1TutorialCheck;
    public bool Game2TutorialCheck;
    public bool Game3TutorialCheck;
    public bool Game4TutorialCheck;
    
    public HashSet<int> MonsterDragIds = new HashSet<int>();
    
    public List<int> ObtainedGachaIDs = new List<int>();
    public List<(int itemId, int itemCount)> ItemId = new List<(int, int)>();
    
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
}
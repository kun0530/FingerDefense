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
    
    public List<int> ObtainedGachaIDs = new List<int>();
    public List<(int itemId, int itemCount)> ItemId = new List<(int, int)>();
}
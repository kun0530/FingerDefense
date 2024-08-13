using System.Collections.Generic;

public interface IResourceManager
{
    public string PlayerName { get; set; }
    bool NicknameCheck { get; set; }
    bool StageChoiceTutorialCheck { get; set; }
    bool DeckUITutorialCheck { get; set; }
    bool Game1TutorialCheck { get; set; }
    bool Game2TutorialCheck { get; set; }
    bool Game3TutorialCheck { get; set; }
    bool Game4TutorialCheck { get; set; }

    int Gold { get; set; }
    int Diamond { get; set; }
    int Ticket { get; set; }
    int Mileage { get; set; }
    List<int> ObtainedGachaIDs { get; set; }
    List<(int itemId, int itemCount)> Items { get; set; }


    void AddItem(int itemId, int itemCount);
    void SaveData();
    void LoadData();
    void RegisterObserver(IResourceObserver observer);
    void RemoveObserver(IResourceObserver observer);
    void NotifyObservers(ResourceType resourceType, int newValue);
}
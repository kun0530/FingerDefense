public interface IResourceSubject
{
    void RegisterObserver(IResourceObserver observer);
    void RemoveObserver(IResourceObserver observer);
    void NotifyObservers(ResourceType resourceType, int newValue);
}
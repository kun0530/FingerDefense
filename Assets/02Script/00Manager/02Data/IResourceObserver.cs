public interface IResourceObserver
{
    void OnResourceUpdate(ResourceType resourceType, int newValue);
}
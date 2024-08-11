using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ResourceManager = gameObject.AddComponent<ResourceManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
    }

    public IResourceManager ResourceManager { get; private set; }
}
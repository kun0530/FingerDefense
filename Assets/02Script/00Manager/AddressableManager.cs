using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class AddressableManager : MonoBehaviour
{
    public Button button;
    
    
    
    public async UniTask<GameObject> LoadPrefabAsync(string key)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(key);
        await handle.Task;
        return handle.Result;
    }
    
    public async UniTask<GameObject> InstantiatePrefabAsync(string key, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = await LoadPrefabAsync(key);
        GameObject go = Instantiate(prefab, position, rotation);
        return go;
    }
    
    public async UniTask<GameObject> InstantiatePrefabAsync(string key, Transform parent, bool worldPositionStays = false)
    {
        GameObject prefab = await LoadPrefabAsync(key);
        GameObject go = Instantiate(prefab, parent, worldPositionStays);
        return go;
    }
    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class AddressableManager : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject[] assetReferenceGameObjects;
    [SerializeField]
    private AssetReference[] assetReferences;
    [SerializeField]
    private AssetReferenceT<AudioClip>[] audioClips;
    [SerializeField]
    private AssetReferenceSprite[] assetReferenceSprites;
    [SerializeField]
    private AssetReferenceT<TextAsset>[] textAssets;
    [SerializeField]
    private AssetReferenceT<PlayableAsset>[] playableAssets;
    [SerializeField]
    private AssetReferenceT<InputActionAsset>[] inputActionAssets;
    
    private List<GameObject> gameObjects = new List<GameObject>();
    
    void Start()
    {
        ButtonClick();
    }

    public void ButtonClick()
    {
        foreach (var t in assetReferenceGameObjects)
        {
            var load = t.LoadAssetAsync<GameObject>();
            load.Completed += (op) =>
            {
                var asset = op.Result;
                Debug.Log(asset.name);
            };
        }
        
        foreach (var t in assetReferences)
        {
            var load = t.LoadAssetAsync<AssetReference>();
            load.Completed += (op) =>
            {
                var asset = op.Result;
                var inst = asset.InstantiateAsync();
                inst.Completed += (op2) =>
                {
                    gameObjects.Add(op2.Result);
                };
            };
        }
        
        foreach (var t in audioClips)
        {
            var load = t.LoadAssetAsync<AudioClip>();
            load.Completed += (op) =>
            {
                var audio = op.Result;
                var audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.clip = audio;
            };
        }
        
        foreach (var t in assetReferenceSprites)
        {
            var load = t.LoadAssetAsync<Sprite>();
            load.Completed += (op) =>
            {
                var sprite = op.Result;
                var image = gameObject.GetComponent<Image>();
                image.sprite = sprite;
            };
        }
        
        foreach (var t in textAssets)
        {
            var load = t.LoadAssetAsync<TextAsset>();
            load.Completed += (op) =>
            {
                var text = op.Result;
                Debug.Log(text.text);
            };
        }
        
        foreach (var t in playableAssets)
        {
            var load = t.LoadAssetAsync<PlayableAsset>();
            load.Completed += (op) =>
            {
                var playable = op.Result;
                Debug.Log(playable.name);
            };
        }

        foreach (var t in inputActionAssets)
        {
            var load = t.LoadAssetAsync<InputActionAsset>();
            load.Completed += (op) =>
            {
                var inputAction = op.Result;
                var action = inputAction.FindAction("Click");
                action.performed += (context) =>
                {
                    Debug.Log("Click");
                };
                
                
                
            };

        }
    }
    

}

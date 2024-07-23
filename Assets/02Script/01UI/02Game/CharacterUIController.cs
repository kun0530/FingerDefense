using UnityEngine;

public class CharacterUIController : MonoBehaviour
{
    public CharacterUIButton characterUISlotPrefab;
    public RectTransform characterUIParent;
    
    private AssetListTable assetListTable;
    
    private void Awake()
    {
        assetListTable ??= DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
    }
    
    private void Start()
    {
        CreateCharacterSlots();
    }
    
    private void CreateCharacterSlots()
    {
        for (int i = 0; i < Variables.LoadTable.characterIds[i]; i++)
        {
            var assetName = assetListTable.Get(Variables.LoadTable.characterIds[i]);
            if (!string.IsNullOrEmpty(assetName))
            {
                //var prefab = Addressables.LoadAssetAsync<GameObject>($"Prefab/00CharacterUI/{assetName}");
                GameObject prefab = Resources.Load<GameObject>($"Prefab/00CharacterUI/{assetName}");

                if (prefab != null)
                {
                    var characterUISlotInstance = Instantiate(characterUISlotPrefab, characterUIParent);
                    var prefabInstance = Instantiate(prefab, characterUISlotInstance.transform);
                }
                    //var prefabInstance = Addressables.DownloadDependenciesAsync(prefab, characterUISlotInstance.transform);
                
            }
            else
            {
                Logger.LogError($"Asset name not found for AssetNo {Variables.LoadTable.characterIds[i]}");
            }
        }
    }
}

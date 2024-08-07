using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;

public class GachaResultSlot : MonoBehaviour
{
    public RectTransform gradeStarParent;
    public GameObject gradeStarPrefab;
    
    public Image duplicationIcon;
    public TextMeshProUGUI duplicationCountText;
    
    public RectTransform CharacterUIParent;
    public void Setup(GachaData data, AssetListTable assetListTable, StringTable stringTable)
    {
        // 등급 별로 별 아이콘 생성
        for (int i = 0; i < data.Grade+1; i++)
        {
            Instantiate(gradeStarPrefab, gradeStarParent);
        }
        
        var assetName = assetListTable.Get(data.AssetNo);
        if (!string.IsNullOrEmpty(assetName))
        {
            var characterObject = Resources.Load<GameObject>($"Prefab/06GachaCharacterUI/{assetName}");
            if (characterObject != null)
            {
                Instantiate(characterObject, CharacterUIParent);
            }
            else
            {
                Logger.LogWarning($"Prefab/06GachaCharacterUI/{assetName} 경로에 게임 오브젝트가 없습니다.");
            }
        }
        else
        {
            Logger.LogWarning($"AssetNo {data.AssetNo}에 해당하는 이름이 AssetListTable에 없습니다.");
        }
        
        
        // 데이터에 따라 아이콘과 기타 정보 설정
        // 예시로 assetListTable과 stringTable에서 필요한 정보를 가져옵니다.
        // duplicationIcon.sprite = assetListTable.GetAsset(data.AssetNo);
        // duplicationCountText.text = stringTable.GetString(data.NameId);
    }
}

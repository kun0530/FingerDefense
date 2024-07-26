using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class PlayerCharacterSpawner : MonoBehaviour
{ 
    public Transform poolTransform;
    public Transform[] spawnPositions; // 6개 (전열: 0, 1 / 중열: 2, 3 / 후열: 4, 5)

    private PlayerCharacterTable playerCharacterTable;
    private SkillTable skillTable;
    private AssetListTable assetListTable;
    
    private PlayerCharacterController[] playerCharacters = new PlayerCharacterController[8];
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6]; // 현재 활성화된 캐릭터 저장
    
    public GameObject playerUICharacterPrefab;
    public RectTransform playerUICharacterParent;
    
    private Button[] characterButtons = new Button[8];
    private void Awake()
    {
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);

        for (var i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            if (Variables.LoadTable.characterIds[i] != 0)
            {
                var data = playerCharacterTable.Get(Variables.LoadTable.characterIds[i]);
                playerCharacters[i] = CreatePlayerCharacter(data);
                CreateUICharacter(data,i);
            }
            else
            {
                CreateUICharacter(null,i);
            }
        }
    }

    private void CreateUICharacter(PlayerCharacterData data,int index)
    {
        var uiCharacter = Instantiate(playerUICharacterPrefab, playerUICharacterParent);
        if (data != null)
        {
            var assetName = assetListTable.Get(data.AssetNo);
            if (string.IsNullOrEmpty(assetName))
            {
                Logger.LogError($"Asset name not found for AssetNo {data.AssetNo}");
                return;
            }

            var uiAsset = Resources.Load<GameObject>($"Prefab/00CharacterUI/{assetName}");
            if (uiAsset == null)
            {
                Logger.LogError($"UI Prefab not found for asset name {assetName}");
                return;
            }

            Instantiate(uiAsset, uiCharacter.transform);
        }
        else
        {
            Logger.LogWarning("Character data is null.");
        }

        var button = uiCharacter.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SpawnPlayerCharacter(index));
            characterButtons[index] = button;
        }
    }

    private PlayerCharacterController CreatePlayerCharacter(PlayerCharacterData data)
    {
        if (data == null)
        {
            Logger.LogError("Character data is null.");
            return null;
        }

        var assetName = assetListTable.Get(data.AssetNo);
        if (string.IsNullOrEmpty(assetName))
        {
            Logger.LogError($"Asset name not found for AssetNo {data.AssetNo}");
            return null;
        }

        var prefab = Resources.Load<GameObject>($"Prefab/02CharacterGame/{assetName}");
        if (prefab == null)
        {
            Logger.LogError($"Prefab not found for asset name {assetName}");
            return null;
        }

        var playerCharacter = Instantiate(prefab, poolTransform).GetComponent<PlayerCharacterController>();
        if (playerCharacter == null)
        {
            Logger.LogError($"PlayerCharacterController not found on prefab {assetName}");
            return null;
        }
        playerCharacter.Status.Data = data;
        // 에러로 인해 비활성화 : 방민호
        // var skillData = skillTable.Get(data.Skill);
        // playerCharacter.skill = SkillFactory.CreateSkill(skillData, playerCharacter.transform);
        // playerCharacter.skillData = skillData;
        if (playerCharacter.TryGetComponent<PlayerAttackBehavior>(out var attackBehavior))
        {
            var normalAttackData = skillTable.Get(data.Skill1);
            var skillAttackData = skillTable.Get(data.Skill2);

            var normalAttack = SkillFactory.CreateSkill(normalAttackData, playerCharacter.gameObject);
            var skillAttack = SkillFactory.CreateSkill(skillAttackData, playerCharacter.gameObject);

            attackBehavior.normalAttack = normalAttack;
            attackBehavior.skillAttack = skillAttack;
        }

        playerCharacter.spawner = this;
        playerCharacter.gameObject.SetActive(false);

        return playerCharacter;
    }

    public void SpawnPlayerCharacter(int index)
    {
        var playerCharacter = playerCharacters[index];

        if (playerCharacter == null)
        {
            Logger.LogError("해당 캐릭터 데이터가 없습니다.");
            return;
        }
        if (playerCharacter.gameObject.activeSelf)
        {
            Logger.LogError("캐릭터가 이미 소환되었습니다.");
            return;
        }
       

        var spawnClass = playerCharacter.Status.Data.Class;
        Logger.Log($"Spawning character with Class {spawnClass}");

        var positionIndex = spawnClass switch
        {
            // 전열
            0 => activePlayerCharacters[0] == null || !activePlayerCharacters[0].gameObject.activeSelf ? 0 : 1,
            // 중열
            1 => activePlayerCharacters[2] == null || !activePlayerCharacters[2].gameObject.activeSelf ? 2 : 3,
            // 후열
            2 => activePlayerCharacters[4] == null || !activePlayerCharacters[4].gameObject.activeSelf ? 4 : 5,
            _ => -1
        };
        
        if (positionIndex == -1 || activePlayerCharacters[positionIndex] != null)
        {
            Logger.Log("해당 위치에 이미 캐릭터가 배치되어 있습니다.");
            return;
        }

        // 캐릭터 위치 설정 후 활성화
        playerCharacter.transform.position = spawnPositions[positionIndex].position;

        activePlayerCharacters[positionIndex] = playerCharacter;
        playerCharacter.Status.Init();
        playerCharacter.gameObject.SetActive(true);
        
        if (characterButtons[index] != null)
        {
            characterButtons[index].interactable = false;
        }
    }

    public void RemoveActiveCharacter(PlayerCharacterController character)
    {
        if (character == null)
            return;

        for (var i = 0; i < activePlayerCharacters.Length; i++)
        {
            if (activePlayerCharacters[i] == character)
            {
                activePlayerCharacters[i].gameObject.SetActive(false);
                activePlayerCharacters[i] = null;
            }
            
            for (var j = 0; j < playerCharacters.Length; j++)
            {
                if (playerCharacters[j] == character && characterButtons[j])
                {
                    characterButtons[j].interactable = true;
                    // UpdateRespawnTimer(characterButtons[j], character.Status.Data.RespawnCoolTime).Forget();
                }
            }
        }
    }

    private async UniTask UpdateRespawnTimer(Button button, float respawnTime)
    {
        float timer = 0f;
        while (timer <= respawnTime)
        {
            // image.fillAmount = timer / respawnTime;
            await UniTask.Yield();
        }
        // image.fillAmount = 1f;
        button.interactable = true;
    }
}
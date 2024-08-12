using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class PlayerCharacterSpawner : MonoBehaviour
{
    public Transform poolTransform;
    public Transform[] spawnPositions;

    private PlayerCharacterTable playerCharacterTable;
    private SkillTable skillTable;
    private AssetListTable assetListTable;

    private PlayerCharacterController[] playerCharacters = new PlayerCharacterController[10];
    private PlayerCharacterController[] activePlayerCharacters = new PlayerCharacterController[6]; // 현재 활성화된 캐릭터 저장

    public GameObject playerUICharacterPrefab;
    public RectTransform playerUICharacterParent;
    private Button[] characterButtons = new Button[10];

    private int selectedCharacterIndex = -1;
    
    private InputManager inputManager;

    private void Awake()
    {
        // 데이터 테이블 로드
        playerCharacterTable = DataTableManager.Get<PlayerCharacterTable>(DataTableIds.PlayerCharacter);
        skillTable = DataTableManager.Get<SkillTable>(DataTableIds.Skill);
        assetListTable = DataTableManager.Get<AssetListTable>(DataTableIds.Asset);
        
        // InputManager 찾기
        inputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        
        // 캐릭터 슬롯 생성
        for (var i = 0; i < Variables.LoadTable.characterIds.Length; i++)
        {
            if (Variables.LoadTable.characterIds[i] != 0)
            {
                var data = playerCharacterTable.Get(Variables.LoadTable.characterIds[i]);
                playerCharacters[i] = CreatePlayerCharacter(data);
                CreateUICharacter(data, i);
            }
            else
            {
                CreateUICharacter(null, i);
            }
        }
    }

    private void OnEnable()
    {
        inputManager.OnTouchStartedEvent += OnTouchStarted;
        inputManager.OnTouchEndedEvent += OnTouchEnded;
    }

    private void OnDisable()
    {
        inputManager.OnTouchStartedEvent -= OnTouchStarted;
        inputManager.OnTouchEndedEvent -= OnTouchEnded;
    }

    private void OnTouchStarted(Vector2 touchPosition)
    {
    }

    private void OnTouchEnded(Vector2 touchPosition)
    {
        if (selectedCharacterIndex == -1)
        {
            return;
        }

        Ray ray = Camera.main!.ScreenPointToRay(touchPosition);
        var layerMask = LayerMask.GetMask("PlayerSpawner");

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            Debug.Log($"Raycast hit: {hit.point}");

            // 검출한 오브젝트의 위치로 캐릭터 소환
            for (var i = 0; i < spawnPositions.Length; i++)
            {
                if (Mathf.Abs(spawnPositions[i].position.x - hit.point.x) 
                    <= 1f && Mathf.Abs(spawnPositions[i].position.y - hit.point.y) <= 1f)
                {
                    Debug.Log($"Spawning character at position index {i}");
                    SpawnPlayerCharacter(i);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any collider.");
        }
    }

    private void CreateUICharacter(PlayerCharacterData data, int index)
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

            Instantiate(uiAsset, uiCharacter.transform).transform.SetAsFirstSibling();
            // uiAsset.transform.SetSiblingIndex(uiAsset.transform.parent.childCount - 1);
        }
        else
        {
            Logger.LogWarning("Character data is null.");
        }

        var button = uiCharacter.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SelectCharacterForSpawning(index));
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

    private void SelectCharacterForSpawning(int index)
    {
        selectedCharacterIndex = index;
        Logger.Log($"Selected character at index {index} for spawning.");
    }
    
    public void SpawnPlayerCharacter(int positionIndex)
    {
        var playerCharacter = playerCharacters[selectedCharacterIndex];
        
        if (selectedCharacterIndex == -1 || playerCharacter == null || playerCharacter.gameObject.activeSelf)
        {
            return;
        }

        if (positionIndex < 0 || positionIndex >= activePlayerCharacters.Length || activePlayerCharacters[positionIndex] != null)
        {
            return;
        }

        playerCharacter.transform.position = spawnPositions[positionIndex].position;

        activePlayerCharacters[positionIndex] = playerCharacter;
        playerCharacter.Status.Init();
        playerCharacter.gameObject.SetActive(true);

        if (characterButtons[selectedCharacterIndex] != null)
        {
            // characterButtons[selectedCharacterIndex].interactable = false;
            var slotButton = characterButtons[selectedCharacterIndex].GetComponent<UiSlotButton>();
            slotButton?.ActiveButton(false);
        }

        selectedCharacterIndex = -1; 
    }

    public void RemoveActiveCharacter(PlayerCharacterController character)
    {
        if (!character)
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
                    // characterButtons[j].interactable = true;
                    UpdateRespawnTimer(characterButtons[j], character.Status.Data.RespawnCoolTime).Forget();
                }
            }
        }
    }

    private async UniTask UpdateRespawnTimer(Button button, float respawnTime)
    {
        float timer = 0f;
        var slotButton = button.GetComponent<UiSlotButton>();

        slotButton?.ActiveButton(false);
        while (timer <= respawnTime)
        {
            timer += Time.deltaTime;
            slotButton?.SetFillAmountBackground(1f - timer / respawnTime);
            await UniTask.Yield();
        }
        slotButton?.ActiveButton(true);
    }
}

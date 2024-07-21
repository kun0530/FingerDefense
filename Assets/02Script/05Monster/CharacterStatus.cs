public class CharacterStatus : IStatus
{
    public PlayerCharacterData data;
    public BuffHandler buffHandler;
    
    //현재 체력
    public float currentHp;
    //초기 체력
    public float maxHp;
    public float currentAtkDmg;
    public float currentAtkSpeed;

    public CharacterStatus(PlayerCharacterData data)
    {
        this.data = data;
        
        buffHandler = new(this);
        Init();
    }

    public void Init()
    {
        if (data == null)
        {
            return; 
        }
        maxHp = data.Hp;
        currentHp = maxHp;
        currentAtkDmg = data.AtkDmg;
        currentAtkSpeed = data.AtkSpeed;
    }

    public void UpdateCurrentState()
    {
        if(data == null)
        {
            return;
        }
        
        currentAtkDmg = data.AtkDmg;
        currentAtkSpeed = data.AtkSpeed;

        foreach (var buff in buffHandler.activeBuffs)
        {
            foreach (var buffAction in buff.BuffActions)
            {
                switch ((BuffType)buffAction.type)
                {
                    case BuffType.ATK_SPEED:
                        currentAtkSpeed += buffAction.value;
                        break;
                    case BuffType.ATK:
                        currentAtkDmg += buffAction.value;
                        break;
                }
            }
        }

        currentAtkDmg = currentAtkDmg < 0f ? 0f : currentAtkDmg;
        currentAtkSpeed = currentAtkSpeed < 0f ? 0f : currentAtkSpeed;

        Logger.Log($"기본 공격력: {data.AtkDmg} / 현재 공격력: {currentAtkDmg}");
    }
}
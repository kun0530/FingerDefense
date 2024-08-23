using UnityEngine;

namespace Defines
{
    public static class Layers
    {
        public static readonly LayerMask DEFAULT_LAYER = 1 << LayerMask.NameToLayer("Default");
        public static readonly LayerMask PLAYER_LAYER = 1 << LayerMask.NameToLayer("Player");
        public static readonly LayerMask MONSTER_LAYER = 1 << LayerMask.NameToLayer("Monster");
    }

    public static class Tags
    {
        public static readonly string STAGE_MANAGER_TAG = "StageManager";
        public static readonly string SOUND_MANAGER_TAG = "SoundManager";

        public static readonly string PLAYER_TAG = "PlayerCharacter";
        public static readonly string MONSTER_TAG = "Monster";

        public static readonly string CASTLE_TAG = "Castle";
        public static readonly string PATROL_LINE_TAG = "PatrolStartLine";
    }

    
    
}
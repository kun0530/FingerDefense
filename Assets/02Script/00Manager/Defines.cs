using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    public static class Laysers
    {
        public static readonly LayerMask DEFAULT_LAYER = 1 << LayerMask.NameToLayer("Default");
        public static readonly LayerMask PLAYER_LAYER = 1 << LayerMask.NameToLayer("Player");
        public static readonly LayerMask MONSTER_LAYER = 1 << LayerMask.NameToLayer("Monster");
    }

    public static class Tags
    {
        public static readonly string PLAYER_TAG = "PlayerCharacter";
        public static readonly string MONSTER_TAG = "Monster";
    }
    
    
}
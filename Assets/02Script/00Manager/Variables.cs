using System.Collections.Generic;
using UnityEngine;

namespace Variables
{
    public static class LoadTable
    {
        //테스트 (스테이지 UI 버튼을 누르면 그 스테이지 id를 넘겨줄 예정)
        //public static int StageId = StageId - 13000;
        public static int StageId;
        //여기다가 슬롯 UI에서 선택한 캐릭터 ID를 넘겨줄 예정
        public static int[] characterIds = new int[8];
    }
    
    public static class CharacterSlotState
    {
        public static List<int> CharacterIds = new List<int>(new int[8]);
        public static List<int> FilteredCharacterIds = new List<int>();
    }
}
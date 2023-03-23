using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene
{
    // GameScene の定数を管理するクラス
    public static class GameSceneConsts
    {
        // 判定時間
        public const float PERFECT_RANGE = 0.05f;
        public const float GREAT_RANGE = 0.075f;
        public const float GOOD_RANGE = 0.1f;
        public const float BAD_RANGE = 0.2f;

        // タップノーツの色
        public static readonly Color32 TAP_NOTE_COLOR = new Color32(0, 0, 0, 0);
    }
}
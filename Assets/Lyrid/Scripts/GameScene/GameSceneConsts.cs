using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene
{
    /// <summary>
    /// GameScene の定数を管理するクラス
    /// </summary>
    public static class GameSceneConsts
    {
        // 判定時間
        public const float PERFECT_RANGE = 0.05f;
        public const float GREAT_RANGE = 0.075f;
        public const float GOOD_RANGE = 0.1f;
        public const float BAD_RANGE = 0.2f;

        // ノーツの色
        public static readonly Color32 TAP_NOTE_COLOR = new Color32(80, 255, 255, 200);
        public static readonly Color32 SWIPE_NOTE_COLOR = new Color32(255, 255, 255, 200);
        public static readonly Color32 FLICK_NOTE_COLOR = new Color32(140, 0, 255, 200);
        public static readonly Color32 SLIDE_NOTE_COLOR = new Color32(230, 180, 80, 200);

        // レーンの高さ
        public const float LANE_HEIGHT = 20.0f;
    }
}
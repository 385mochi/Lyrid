using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.Common
{
    /// <summary>
    /// Lyrid 全体で使用される定数を管理するクラス
    /// </summary>
    public static class CommonConsts
    {
        // 難易度
        public enum Difficulty { None, Normal, Hard, Expert };

        // 難易度の色
        public static readonly Color32 NORMAL_DIFF_COLOR = new Color32(120, 255, 255, 255);
        public static readonly Color32 HARD_DIFF_COLOR = new Color32(255, 200, 0, 255);
        public static readonly Color32 EXPERT_DIFF_COLOR = new Color32(150, 0, 255, 255);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LyridEditor.Common
{
    /// <summary>
    /// LyridEditpr で使用される定数を管理するクラス
    /// </summary>
    public static class CommonConsts
    {
        /// <summary> レーンの色 </summary>
        public static readonly Color32[] LANE_COLOR = {
            new Color32(255, 0, 0, 255),   // レーン1(赤)
            new Color32(255, 127, 0, 255), // レーン2(オレンジ)
            new Color32(255, 255, 0, 255), // レーン3(黄)
            new Color32(127, 255, 0, 255), // レーン4(黄緑)
            new Color32(0, 255, 0, 255),   // レーン5(緑)
            new Color32(0, 255, 255, 255), // レーン6(水色)
            new Color32(0, 127, 255, 255), // レーン7(青)
            new Color32(0, 0, 255, 255),   // レーン8(濃い青)
            new Color32(127, 0, 255, 255), // レーン9(紫)
            new Color32(255, 0, 255, 255)  // レーン10(ピンク)
        };
    }
}
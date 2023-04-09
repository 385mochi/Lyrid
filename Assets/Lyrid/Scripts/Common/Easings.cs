using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Lyrid.Common
{
    /// <summary>
    /// Easings を管理するクラス
    /// </summary>
    public static class Easings
    {
        /// <summary>
        /// Easing のリスト
        /// </summary>
        private static Ease[] easings ={
            Ease.Unset,
            Ease.Linear,

            Ease.InSine,
            Ease.OutSine,
            Ease.InOutSine,

            Ease.InQuad,
            Ease.OutQuad,
            Ease.InOutQuad,

            Ease.InCubic,
            Ease.OutCubic,
            Ease.InOutCubic,

            Ease.InQuart,
            Ease.OutQuart,
            Ease.InOutQuart,

            Ease.InQuint,
            Ease.OutQuint,
            Ease.InOutQuint,

            Ease.InExpo,
            Ease.OutExpo,
            Ease.InOutExpo,

            Ease.InCirc,
            Ease.OutCirc,
            Ease.InOutCirc,

            Ease.InBack,
            Ease.OutBack,
            Ease.InOutBack,

            Ease.InElastic,
            Ease.OutElastic,
            Ease.InOutElastic,

            Ease.InBounce,
            Ease.OutBounce,
            Ease.InOutBounce,
        };

        /// <summary>
        /// 番号に対応する Easing を返すメソッド
        /// </summary>
        /// <param name="num"> 番号 </param>
        /// <returns> 対応する Easing </returns>
        public static Ease GetEaseType(int num)
        {
            // 範囲外であれば Unset を返す
            if (num < 0 || 31 < num)
            {
                return easings[0];
            }
            else
            {
                return easings[num];
            }
        }
    }
}
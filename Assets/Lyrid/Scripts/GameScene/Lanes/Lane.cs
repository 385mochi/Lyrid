using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lyrid.GameScene.Charts;
using static Lyrid.Common.Easings;

namespace Lyrid.GameScene.Lanes
{
    /// <summary>
    /// レーンのクラス
    /// </summary>
    public class Lane : MonoBehaviour
    {
        #region Field
        /// <summary> レーンの光のオブジェクト </summary>
        [SerializeField] private LaneLight laneLight;
        /// <summary> レーンのエフェクトのオブジェクト </summary>
        [SerializeField] private LaneEffectLight laneEffectLight;
        #endregion

        #region Methods
        /// <summary>
        /// レーンの可視状態を更新するメソッド
        /// </summary>
        /// <param name="b"> 可視化するかどうか </param>
        public void SetVisible(bool b)
        {
            gameObject.SetActive(b);
        }

        /// <summary>
        /// レーンを光らせるメソッド
        /// </summary>
        public void LightUp()
        {
            laneLight.LightUp();
        }

        /// <summary>
        /// レーンのエフェクトを光らせるメソッド
        /// </summary>
        /// <param name="type"> 判定したノートの種類 </param>
        /// <param name="width"> レーン幅を 1 としたときの幅 </param>
        public void EffectLightUp(ElementType type, float width)
        {
            laneEffectLight.LightUp(type, width);
        }

        /// <summary>
        /// レーンを移動させるメソッド
        /// </summary>
        /// <param name="posX"> 目標となる位置 </param>
        /// <param name="option"> 移動の </param>
        /// <param name="t"> 移動時間 </param>
        /// <param name="delay"> 遅延時間 </param>
        public void Move(float t,  float delay, float posX, int option)
        {
            if (option > 0)
            {
                gameObject.transform.DOMoveX(posX, t)
                .SetDelay(delay)
                .SetEase(GetEaseType(option));
            }
        }

        /// <summary>
        /// レーン幅を変化させるメソッド
        /// </summary>
        /// <param name="scale"> 目標となる幅 </param>
        /// <param name="option"> 移動の </param>
        /// <param name="t"> 移動時間 </param>
        /// <param name="delay"> 遅延時間 </param>
        public void Scale(float t,  float delay, float scale, int option)
        {
            if (option > 0)
            {
                gameObject.transform.DOScaleX(scale, t)
                .SetDelay(delay)
                .SetEase(GetEaseType(option));
            }
        }
        #endregion
    }
}
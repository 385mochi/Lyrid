using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        [SerializeField] private GameObject laneLight;
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
            laneLight.GetComponent<LaneLight>().LightUp();
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
                gameObject.transform.DOMoveX(posX, t).SetDelay(delay).SetEase(GetEaseType(option));
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
                gameObject.transform.DOScaleX(scale, t).SetDelay(delay).SetEase(GetEaseType(option));
            }
        }
        #endregion
    }
}
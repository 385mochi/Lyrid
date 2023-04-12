using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lyrid.GameScene;

namespace Lyrid.GameScene.UI
{
    /// <summary>
    /// 楽曲の再生位置を示すタイムラインのクラス
    /// </summary>
    public class TimeLine : MonoBehaviour
    {
        #region Field
        ///  <summary> タイムラインとなるスライダー </summary>
        [SerializeField] private Slider timeLine;
        private float lengthInverse;
        private float prevTime;
        #endregion

        #region Methods
        /// <summary>
        /// タイムラインを更新するメソッド
        /// </summary>
        /// <param name="time"> 更新値 </param>
        public void UpdateLine(float time)
        {
            if (prevTime < time)
            {
                timeLine.value = time * lengthInverse;
            }
            prevTime = time;
        }

        /// <summary>
        /// タイムラインをリセットするメソッド
        /// </summary>
        public void Reset()
        {
            timeLine.value = 0;
            prevTime = 0;
        }

        public void SetLength(float length)
        {
            lengthInverse = 1.0f / length;
        }
        #endregion
    }
}
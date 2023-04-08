using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Lanes
{
    /// <summary>
    /// レーンのクラス
    /// </summary>
    public class Lane : MonoBehaviour
    {
        #region Field
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
        #endregion
    }
}
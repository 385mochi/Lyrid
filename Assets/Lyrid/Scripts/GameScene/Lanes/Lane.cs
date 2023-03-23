using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Lanes
{
    // レーンのクラス
    public class Lane : MonoBehaviour
    {
        #region Field
        [SerializeField] GameObject laneLight;
        #endregion

        #region Methods
        public void SetVisible(bool b)
        {
            gameObject.SetActive(b);
        }

        public void LightUp()
        {
            laneLight.GetComponent<LaneLight>().LightUp();
        }
        #endregion
    }
}
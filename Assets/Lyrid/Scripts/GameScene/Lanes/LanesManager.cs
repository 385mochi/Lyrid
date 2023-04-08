using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Lanes
{
    /// <summary>
    /// レーンの管理を行うクラス
    /// </summary>
    public class LanesManager : MonoBehaviour
    {
        #region Field
        /// <summary> レーンオブジェクトの Prefab </summary>
        [SerializeField] private GameObject lanePrefab;
        /// <summary> レーンの最大数 </summary>
        private int laneNum;
        /// <summary> レーンの初期数 </summary>
        private int initlaneNum;
        /// <summary> レーンの初期幅 </summary>
        private float laneWidth;
        /// <summary> レーンの可視化の初期状態 </summary>
        private bool setVisible;
        #endregion

        #region Property
        /// <summary> レーンオブジェクトのリスト </summary>
        public List<GameObject> laneObjects { get; private set; }
        /// <summary> レーンの Transform のリスト </summary>
        public List<Transform> laneTransforms { get; private set; }
        /// <summary> Lane のリスト </summary>
        public List<Lane> lanes { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// レーンを初期状態にするメソッド
        /// </summary>
        public void Init()
        {
            if (laneObjects != null)
            {
                foreach (GameObject laneObj in laneObjects)
                {
                    Destroy(laneObj);
                }
            }
            SetLanes(laneNum, initlaneNum, laneWidth, setVisible);
        }

        /// <summary>
        /// レーンを生成するメソッド
        /// </summary>
        /// <param name="laneNum"> 最大レーン数 </param>
        /// <param name="initlaneNum"> 初期レーン数 </param>
        /// <param name="laneWidth"> 初期レーン幅</param>
        /// <param name="setVisible"> 可視化するかどうか </param>
        public void SetLanes(int laneNum, int initlaneNum, float laneWidth, bool setVisible)
        {
            this.laneNum = laneNum;
            this.initlaneNum = initlaneNum;
            this.laneWidth = laneWidth;
            this.setVisible = setVisible;

            laneObjects = new List<GameObject>();
            laneTransforms = new List<Transform>();
            lanes= new List<Lane>();

            // レーンの x 座標
            float posX = -(initlaneNum - 1) * laneWidth / 2.0f;
            float delay = 2.0f;
            for (int i = 0; i < laneNum; i++)
            {
                GameObject lane;
                if (i < initlaneNum)
                {
                    delay += 0.1f;
                    lane = Instantiate(
                        lanePrefab,
                        new Vector3(posX, 0.0f, 0.0f),
                        Quaternion.Euler(90f, 0, 0),
                        gameObject.transform
                    );
                    if (!setVisible)
                    {
                        lane.GetComponent<Lane>().SetVisible(false);
                    }
                }
                else
                {
                    lane = Instantiate(
                        lanePrefab,
                        new Vector3(0.0f, 0.0f, 0.0f),
                        Quaternion.Euler(90f, 0, 0),
                        gameObject.transform
                    );
                    lane.GetComponent<Lane>().SetVisible(false);
                }
                laneObjects.Add(lane);
                laneTransforms.Add(lane.transform);
                lanes.Add(lane.GetComponent<Lane>());
                lane.transform.localScale = new Vector3(
                    lane.transform.localScale.x * laneWidth,
                    lane.transform.localScale.y,
                    lane.transform.localScale.z
                );
                posX += laneWidth;
            }
        }

        //TODO: Move を実装
        // index 番目のレーンを位置 pos へ 時間 t で移動させるメソッド
        public void Move(int index, float pos, float t, int option, float delay) {
            /*
            if(option == 0) return;
            iTween.EaseType easeType = iTween.EaseType.linear;
            if(option == 2) easeType = iTween.EaseType.easeInSine;
            if(option == 3) easeType = iTween.EaseType.easeOutSine;
            if(option == 4) easeType = iTween.EaseType.easeInOutSine;
            iTween.MoveTo(
                laneObjects[index],
                iTween.Hash("x", pos, "time", t, "easetype", easeType, "delay", delay)
            );
            */
        }

        //TODO: Scale を実装
        // index 番目のレーンを幅 width へ 時間 t で変化させる
        public void Scale(int index, float width, float t, int option, float delay) {
            /*
            if(option == 0) return;
            iTween.EaseType easeType = iTween.EaseType.linear;
            if(option == 2) easeType = iTween.EaseType.easeInSine;
            if(option == 3) easeType = iTween.EaseType.easeOutSine;
            if(option == 4) easeType = iTween.EaseType.easeInOutSine;
            // width が 0 のときはアニメーション完了後不可視にする
            if(width == 0)
            {
                iTween.ScaleTo(
                    laneObjects[index],
                    iTween.Hash("x", width,
                    "time", t,
                    "easetype", easeType,
                    "delay", delay,
                    "onstarttarget", gameObject,
                    "onstartparams", index,
                    "onstart", "SetVisibleTrue",
                    "oncompletetarget", gameObject,
                    "oncompleteparams", index,
                    "oncomplete", "SetVisibleFalse")
                );
            }
            else
            {
                iTween.ScaleTo(
                    laneObjects[index],
                    iTween.Hash("x", width,
                    "time", t,
                    "easetype", easeType,
                    "delay", delay,
                    "onstarttarget", gameObject,
                    "onstartparams", index,
                    "onstart", "SetVisibleTrue")
                );
            }
            */
        }
        #endregion
    }
}
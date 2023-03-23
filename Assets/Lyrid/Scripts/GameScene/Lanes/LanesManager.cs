using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Lanes
{
    // レーンの管理を行うクラス
    public class LanesManager : MonoBehaviour
    {
        #region Field
        // レーンオブジェクトの Prefab
        [SerializeField] GameObject lanePrefab;
        // レーンオブジェクトのリスト
        public List<GameObject> laneObjects = new List<GameObject>();
        // Transform のリスト
        public List<Transform> laneTransforms = new List<Transform>();
        // Lane のリスト
        public List<Lane> lanes = new List<Lane>();
        // レーンの最大数
        private int laneNum;
        // レーンの初期数
        private int initlaneNum;
        // レーンの初期幅
        private float laneWidth;
        // レーンの可視化の初期状態
        private bool setVisible;
        #endregion

        #region Methods
        // レーンを初期状態にするメソッド
        public void Reset()
        {
            if (laneObjects != null)
            {
                foreach (GameObject laneObj in laneObjects)
                {
                    Destroy(laneObj);
                }
            }
            laneObjects = new List<GameObject>();
            laneTransforms = new List<Transform>();
            SetLanes(laneNum, initlaneNum, laneWidth, setVisible);
        }

        // レーンを生成するメソッド
        public void SetLanes(int laneNum, int initlaneNum, float laneWidth, bool setVisible)
        {
            this.laneNum = laneNum;
            this.initlaneNum = initlaneNum;
            this.laneWidth = laneWidth;
            this.setVisible = setVisible;
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
                /*
                iTween.ScaleFrom(
                    lane,
                    iTween.Hash(
                        "y", 0.0f,
                        "time", 3.0f,
                        "delay", delay
                    )
                );
                */
                posX += laneWidth;
            }
        }

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

        public void SetVisibleTrue(int index) {
            laneObjects[index].GetComponent<Lane>().SetVisible(true);
        }

        public void SetVisibleFalse(int index) {
            laneObjects[index].GetComponent<Lane>().SetVisible(false);
        }

        public void LightUp(int index) {
            if(index < 0) return;
            laneObjects[index].GetComponent<Lane>().LightUp();
        }
        #endregion
    }
}
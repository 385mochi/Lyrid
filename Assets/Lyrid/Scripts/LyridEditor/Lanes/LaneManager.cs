using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LyridEditor.Bars;
using LyridEditor.UI;

namespace LyridEditor.Lanes
{
    /// <summary>
    /// 全てのレーンを管理するクラス
    /// </summary>
    public class LaneManager : MonoBehaviour
    {
        private int laneNum = 10;
        private List<Lane> laneList;
        private Palette palette;
        private BarManager barManager;
        private Transform barsTransform;
        [SerializeField] private GameObject lanePrefab;

        public int selectedIndex { get; private set; }

        void Start()
        {
            palette = GameObject.Find("Palette").GetComponent<Palette>();
            barManager = GameObject.Find("Bars").GetComponent<BarManager>();
            barsTransform = GameObject.Find("Bars").transform;
            selectedIndex = 0;
            Init();
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Init()
        {
            laneList = new List<Lane>();
            for (int i = 0; i < laneNum; i++)
            {
                GameObject laneObj = Instantiate(lanePrefab, this.transform);
                laneObj.name = $"Lane{i + 1}";
                Lane lane = laneObj.GetComponent<Lane>();
                lane.Init(i);
                laneList.Add(lane);
            }
        }

        /// <summary>
        /// 選択レーンを切り替える
        /// </summary>
        /// <param name="index"> レーンのインデックス </param>
        public void SwitchLane(int index)
        {
            for (int i = 0; i < laneList.Count; i++)
            {
                laneList[i].UpdateColor(!(i == index));
            }
            if (index < 0 || 10 <= index)
            {
                return;
            }
            selectedIndex = index;
        }

        /// <summary>
        /// レーンユニットを追加する
        /// </summary>
        /// <param name="worldPos"> 追加するワールド座標 </param>
        public void AddLaneUnit(Vector3 worldPos)
        {
            // Bars 基準でワールド座標からローカル座標に変換
            Vector3 localPos = barsTransform.InverseTransformPoint(worldPos);
            var id = barManager.LocalPosToBarLineId(localPos);
            if (id == null)
            {
                return;
            }

            // 指定された部分に レーンユニットを追加
            Lane lane = laneList[selectedIndex];
            lane.AddLaneUnit(id!.Value.Item1, id!.Value.Item2, localPos.x);
        }

        /// <summary>
        /// 小節が追加されたときの処理
        /// </summary>
        public void AddBar()
        {
            for (int i = 0; i < laneList.Count; i++)
            {
                laneList[i].AddBar();
            }
        }
    }
}
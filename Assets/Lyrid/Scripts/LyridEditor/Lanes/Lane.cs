using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LyridEditor.Bars;

namespace LyridEditor.Lanes
{
    /// <summary>
    /// レーンユニットをまとめるクラス
    /// </summary>
    public class Lane : MonoBehaviour
    {
        public List<LaneUnit> laneUnitList;
        [SerializeField] private GameObject laneUnitPrefab;
        private int laneIndex;

        public void Init(int index)
        {
            laneIndex = index;
            laneUnitList = new List<LaneUnit>();
        }

        /// <summary>
        /// レーンユニットを追加する
        /// </summary>
        /// <param name="barId"> 小節の ID </param>
        /// <param name="lineId"> 分割線の ID </param>
        /// <param name="posX"> 追加する位置の Bars を基準とした x 座標 </param>
        public void AddLaneUnit(int barId, int lineId, float posX)
        {
            // すでに置かれている場合は何もしない
            if (IsPlaced(barId, lineId))
            {
                return;
            }

            // 一番先頭に追加する場合
            if (barId == 0 && lineId == 0)
            {
                // レーンユニットがまだない場合
                if (laneUnitList.Count == 0)
                {
                    GameObject laneUnitObj = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit = laneUnitObj.GetComponent<LaneUnit>();
                    laneUnit.Init(laneIndex);
                    laneUnit.UpdateStartPos(barId, lineId, posX);
                    laneUnit.UpdateEndPos(-1, -1, posX);
                    laneUnit.UpdateColor(laneIndex, false);
                    AddToLaneUnitList(laneUnit);
                }
                // レーンユニットがすでにある場合は先頭に StartLine が存在するので追加できない
            }
            // 先頭以外に追加する場合
            else
            {
                // レーンユニットがまだない場合
                if (laneUnitList.Count == 0)
                {
                    // レーンユニット前半
                    GameObject laneUnitObj_1 = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit_1 = laneUnitObj_1.GetComponent<LaneUnit>();
                    laneUnit_1.Init(laneIndex);
                    laneUnit_1.UpdateStartPos(0, 0, posX);
                    laneUnit_1.UpdateEndPos(barId, lineId, posX);
                    laneUnit_1.UpdateColor(laneIndex, false);
                    AddToLaneUnitList(laneUnit_1);

                    // レーンユニット後半
                    GameObject laneUnitObj_2 = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit_2 = laneUnitObj_2.GetComponent<LaneUnit>();
                    laneUnit_2.Init(laneIndex);
                    laneUnit_2.UpdateStartPos(barId, lineId, posX);
                    laneUnit_2.UpdateEndPos(-1, -1, posX);
                    laneUnit_2.UpdateColor(laneIndex, false);
                    AddToLaneUnitList(laneUnit_2);
                }
                // レーンユニットがすでにある場合
                else
                {
                    // 追加位置に最も近いレーンユニットのインデックスを取得
                    int prevLaneUnitIndex = laneUnitList.Count - 1;
                    for(int i = 0; i < laneUnitList.Count; i++)
                    {
                        if (
                            ((laneUnitList[i].startBarId <= barId && laneUnitList[i].startLineId < lineId) || laneUnitList[i].startBarId < barId) &&
                            ((barId <= laneUnitList[i].endBarId && lineId < laneUnitList[i].endLineId) || barId < laneUnitList[i].endBarId)
                        )
                        {
                           prevLaneUnitIndex = i;
                        }
                    }

                    // レーンユニットを取得し、リストから削除
                    LaneUnit prevLaneUnit = laneUnitList[prevLaneUnitIndex];
                    laneUnitList.RemoveAt(prevLaneUnitIndex);

                    // レーンユニット前半
                    GameObject laneUnitObj_1 = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit_1 = laneUnitObj_1.GetComponent<LaneUnit>();
                    laneUnit_1.Init(laneIndex);
                    laneUnit_1.UpdateStartPos(prevLaneUnit.startBarId, prevLaneUnit.startLineId, prevLaneUnit.startPosX);
                    laneUnit_1.UpdateEndPos(barId, lineId, posX);
                    laneUnit_1.UpdateColor(laneIndex, false);
                    AddToLaneUnitList(laneUnit_1);
                    // レーンユニット後半
                    GameObject laneUnitObj_2 = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit_2 = laneUnitObj_2.GetComponent<LaneUnit>();
                    laneUnit_2.Init(laneIndex);
                    laneUnit_2.UpdateStartPos(barId, lineId, posX);
                    laneUnit_2.UpdateEndPos(prevLaneUnit.endBarId, prevLaneUnit.endLineId, prevLaneUnit.endPosX);
                    laneUnit_2.UpdateColor(laneIndex, false);
                    AddToLaneUnitList(laneUnit_2);

                    Destroy(prevLaneUnit.gameObject);
                }
            }
        }

        /// <summary>
        /// 小節が追加されたときの処理
        /// </summary>
        public void AddBar()
        {
            if (laneUnitList.Count == 0)
            {
                // レーンユニットがない場合は何もしない
                return;
            }
            else
            {
                // 末尾のレーンユニットを取得
                LaneUnit lastLaneUnit = laneUnitList[laneUnitList.Count-1];

                // 始点と終点の x 座標が同じなら延長
                if (lastLaneUnit.startPosX == lastLaneUnit.endPosX)
                {
                    lastLaneUnit.UpdateEndPos(-1, -1, lastLaneUnit.endPosX);
                }
                // 同じでなければ新たに追加
                else
                {
                    BarManager barManager = GameObject.Find("Bars").GetComponent<BarManager>();
                    LaneManager laneManager = GameObject.Find("Lanes").GetComponent<LaneManager>();

                    // 末尾のレーンユニットの EndPos を更新
                    lastLaneUnit.UpdateEndPos(barManager.barNum - 1, 0, lastLaneUnit.endPosX);

                    // レーンユニットを追加
                    GameObject laneUnitObj = Instantiate(laneUnitPrefab, this.transform);
                    LaneUnit laneUnit = laneUnitObj.GetComponent<LaneUnit>();
                    laneUnit.Init(laneIndex);
                    laneUnit.UpdateStartPos(barManager.barNum - 1, 0, lastLaneUnit.endPosX);
                    laneUnit.UpdateEndPos(-1, -1, lastLaneUnit.endPosX);
                    laneUnit.UpdateColor(laneIndex, !(laneManager.selectedIndex == laneIndex));
                    AddToLaneUnitList(laneUnit);
                }
            }
        }

        /// <summary>
        /// レーンの色を更新する
        /// </summary>
        /// <param name="faded"> 色を薄くするかどうか </param>
        public void UpdateColor(bool faded)
        {
            for (int i = 0; i < laneUnitList.Count; i++)
            {
                laneUnitList[i].UpdateColor(laneIndex, faded);
            }
        }

        /// <summary>
        /// 順序を保ったままレーンユニットをリストに追加する
        /// </summary>
        /// <param name="laneUnit"> 追加するレーンユニット </param>
        private void AddToLaneUnitList(LaneUnit laneUnit)
        {
            for(int i = 0; i < laneUnitList.Count; i++)
            {
                if (
                    (laneUnit.startBarId <= laneUnitList[i].startBarId && laneUnit.startLineId < laneUnitList[i].startLineId) ||
                    laneUnit.startBarId < laneUnitList[i].startBarId
                )
                {
                    laneUnitList.Insert(i, laneUnit);
                    return;
                }
            }
            laneUnitList.Add(laneUnit);
        }

        /// <summary>
        /// すでにレーンユニットの開始点がおかれているかどうか判定する
        /// </summary>
        /// <param name="barId"> 小節の ID </param>
        /// <param name="lineId"> 分割線の ID </param>
        /// <returns></returns>
        private bool IsPlaced(int barId, int lineId)
        {
            for(int i = 0; i < laneUnitList.Count; i++)
            {
                if (laneUnitList[i].startBarId == barId && laneUnitList[i].startLineId == lineId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
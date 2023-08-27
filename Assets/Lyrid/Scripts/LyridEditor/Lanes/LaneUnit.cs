using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LyridEditor.Bars;
using static LyridEditor.Common.CommonConsts;

namespace LyridEditor.Lanes
{
    /// <summary>
    /// 1単位分のレーンを管理するクラス
    /// </summary>
    public class LaneUnit : MonoBehaviour
    {
        [SerializeField] private GameObject startLineObj;
        [SerializeField] private GameObject endLineObj;
        [SerializeField] private GameObject middleLineObj;
        private BarManager barManager;
        private int laneIndex;

        public int startBarId { get; private set;}
        public int startLineId { get; private set;}
        public float startPosX { get; private set;}
        public float startWidth { get; private set;}
        public int endBarId { get; private set;}
        public int endLineId { get; private set;}
        public float endPosX { get; private set;}
        public float endWidth { get; private set;}

        public void Init(int laneIndex)
        {
            barManager = GameObject.Find("Bars").GetComponent<BarManager>();
            startLineObj.GetComponent<StartLine>().Init(laneIndex, this);
            endLineObj.GetComponent<EndLine>().Init(laneIndex, this);
        }

        public void UpdateStartPos(int barId, int lineId, float posX)
        {
            float? posY = barManager.BarLineIdToLocalPosY(barId, lineId);
            if (posY == null)
            {
                return;
            }
            startLineObj.transform.localPosition = new Vector3(posX, posY!.Value, 0);
            startBarId = barId;
            startLineId = lineId;
            startPosX = posX;
            UpdateMiddleLine();
        }

        public void UpdateStartWidth(float width)
        {
            startLineObj.GetComponent<StartLine>().UpdateWidth(width);
            startWidth = width;
            UpdateMiddleLine();
        }

        public void UpdateEndPos(int barId, int lineId, float posX)
        {
            float? posY = barManager.BarLineIdToLocalPosY(barId, lineId);
            if (posY == null)
            {
                return;
            }
            endLineObj.transform.localPosition = new Vector3(posX, posY!.Value, 0);
            endBarId = barId;
            endLineId = lineId;
            endPosX = posX;
            UpdateMiddleLine();
        }

        public void UpdateEndWidth(float width)
        {
            endWidth = width;
            UpdateMiddleLine();
        }

        public void MoveStartPos()
        {
            Debug.Log("MoveStartPos");
        }

        public void MoveEndPos()
        {
            Debug.Log("MoveEndPos");
        }

        private void UpdateMiddleLine()
        {
            MiddleLine middleLine = middleLineObj.GetComponent<MiddleLine>();
            middleLine.UpdateLine();
        }

        /// <summary>
        /// 色を変更する
        /// </summary>
        /// <param name="index"> 色のインデックス </param>
        /// <param name="faded"> 色を薄くするかどうか </param>
        public void UpdateColor(int index, bool faded)
        {
            Color32 color = LANE_COLOR[index];
            if (faded)
            {
                color = new Color32(color.r, color.g, color.b, 10);
            }
            else
            {
                color = new Color32(color.r, color.g, color.b, 127);
            }
            startLineObj.GetComponent<StartLine>().UpdateColor(color);
            endLineObj.GetComponent<EndLine>().UpdateColor(color);
            middleLineObj.GetComponent<MiddleLine>().UpdateColor(color);
        }
    }
}
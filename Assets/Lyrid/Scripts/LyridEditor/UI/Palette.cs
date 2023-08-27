using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LyridEditor.Lanes;
using static LyridEditor.Common.CommonConsts;

namespace LyridEditor.UI
{
    /// <summary>
    /// パレットを管理するクラス
    /// </summary>
    public class Palette : MonoBehaviour
    {
        /// <summary> ポインターモードのフレーム </summary>
        [SerializeField] private GameObject pointerButtonFrame;
        /// <summary> レーン設置モードのフレーム </summary>
        [SerializeField] private GameObject laneButtonFrame;
        /// <summary> ノート設置モードのフレーム </summary>
        [SerializeField] private GameObject noteButtonFrame;
        /// <summary> スライドノート設置モードのフレーム </summary>
        [SerializeField] private GameObject slideNoteButtonFrame;
        /// <summary> パレットのレーンのアイコン(線部分) </summary>
        [SerializeField] private Image laneIconLine;
        /// <summary> パレットのレーンのアイコン(円部分) </summary>
        [SerializeField] private Image laneIconCircle;

        /// <summary> パレットの状態列挙型 </summary>
        public enum Status { None, Pointer, Lane, Note, SlideNote }
        /// <summary> パレットの状態変数 </summary>
        public Status status { get; private set; }
        /// <summary> 選択中のレーンのインデックス </summary>
        public int laneIndex { get; private set; }

        public void Reset()
        {
            status = Status.None;
            pointerButtonFrame.SetActive(false);
            laneButtonFrame.SetActive(false);
            noteButtonFrame.SetActive(false);
            slideNoteButtonFrame.SetActive(false);
        }

        public void Select(int index)
        {
            Reset();
            switch(index)
            {
                case 0:
                    status = Status.None;
                    break;
                case 1:
                    status = Status.Pointer;
                    pointerButtonFrame.SetActive(true);
                    break;
                case 2:
                    status = Status.Lane;
                    laneButtonFrame.SetActive(true);
                    GameObject.Find("Lanes").GetComponent<LaneManager>().SwitchLane(laneIndex);
                    break;
                case 3:
                    status = Status.Note;
                    noteButtonFrame.SetActive(true);
                    break;
                case 4:
                    status = Status.SlideNote;
                    slideNoteButtonFrame.SetActive(true);
                    break;
            }
        }

        public void SwitchLane(int index)
        {
            Color32 color = LANE_COLOR[index];
            laneIconLine.color = color;
            laneIconCircle.color = color;
            laneIndex = index;
        }
    }
}
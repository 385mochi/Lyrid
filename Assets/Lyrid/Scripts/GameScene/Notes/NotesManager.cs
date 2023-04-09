using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Lanes;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノーツを生成・管理するクラス
    /// </summary>
    public class NotesManager
    {
        #region Field
        /// <summary> 生成時間のリストのインデックス </summary>
        private int timeDataIndex;
        /// <summary> スライドノートのリストのインデックス </summary>
        private int slideNoteIndex;
        /// <summary> ノートの落下速度 </summary>
        private float speed;
        /// <summary> レーンの高さ </summary>
        private float laneHeight;
        /// <summary> 譜面のインスタンス </summary>
        private Chart chart;
        /// <summary> MovementManager のインスタンス </summary>
        private MovementManager movementManager;
        /// <summary> JudgementManager のインスタンス </summary>
        private JudgementManager judgementManager;
        /// <summary> LanesManager のインスタンス </summary>
        private LanesManager lanesManager;
        #endregion

        #region Constructor
        /// <param name="chart"> 譜面クラスのインスタンス </param>
        /// <param name="movementManager"> MovementManager のインスタンス </param>
        /// <param name="judgementManager"> JudgementManager のインスタンス </param>
        public NotesManager(Chart chart, MovementManager movementManager, JudgementManager judgementManager)
        {
            this.chart = chart;
            speed = chart.initSpeed;
            timeDataIndex = 0;
            slideNoteIndex = 0;
            this.movementManager = movementManager;
            this.judgementManager = judgementManager;
            lanesManager = GameObject.Find("Lanes").GetComponent<LanesManager>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        public void ManagedUpdate(float time)
        {
            // index を進めながら、生成する時間に到達したらノーツを生成する
            while (
                timeDataIndex < chart.timeData.Count &&
                chart.timeData[timeDataIndex] <= time + (10 - speed) * 0.5f
            )
            {
                GenerateNote(time, timeDataIndex);
                timeDataIndex++;
            }
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            speed = chart.initSpeed;
            timeDataIndex = 0;
            slideNoteIndex = 0;
        }

        /// <summary>
        /// index 番目のノーツを生成するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <param name="index"> 生成ノーツのインデックス </param>
        private void GenerateNote(float time, int index)
        {
            float generatedTime = time;
            float judgementTime = chart.timeData[index];
            NoteParam noteParam = chart.notesData[index];
            // ノーツを生成
            switch (noteParam.type)
            {
                case ElementType.Tap:
                    TapNote tapNote = new TapNote(generatedTime, judgementTime, noteParam, false);
                    movementManager.AddTarget(tapNote);
                    judgementManager.AddTarget(tapNote);
                    break;
                case ElementType.Swipe:
                    SwipeNote swipeNote = new SwipeNote(generatedTime, judgementTime, noteParam, false);
                    movementManager.AddTarget(swipeNote);
                    judgementManager.AddTarget(swipeNote);
                    break;
                case ElementType.Flick:
                    FlickNote flickNote = new FlickNote(generatedTime, judgementTime, noteParam, false);
                    movementManager.AddTarget(flickNote);
                    judgementManager.AddTarget(flickNote);
                    break;
                case ElementType.Slide:
                    // slideNoteIndexList[slideNoteIndex] の最初のインデックスでなければ何もしない
                    if (slideNoteIndex >= chart.slideNoteIndexList.Count || index != chart.slideNoteIndexList[slideNoteIndex][0])
                    {
                        break;
                    }
                    // 判定時間とパラメータのリストを作成
                    int size = chart.slideNoteIndexList[slideNoteIndex].Count;
                    List<float> judgementTimeList = new List<float>(size);
                    List<NoteParam> noteParamList = new List<NoteParam>(size);
                    for (int i = 0; i < size; i++)
                    {
                        judgementTimeList.Add(chart.timeData[chart.slideNoteIndexList[slideNoteIndex][i]]);
                        noteParamList.Add(chart.notesData[chart.slideNoteIndexList[slideNoteIndex][i]]);
                    }
                    SlideNote slideNote = new SlideNote(generatedTime, judgementTimeList, noteParamList, movementManager, judgementManager);
                    movementManager.AddTarget(slideNote);
                    judgementManager.AddTarget(slideNote);
                    slideNoteIndex++;
                    break;
                case ElementType.LanePos:
                    if (chart.lanePosIndexList[noteParam.laneNum].Count > 1)
                    {
                        float t = chart.timeData[chart.lanePosIndexList[noteParam.laneNum][1]] - chart.timeData[chart.lanePosIndexList[noteParam.laneNum][0]];
                        float delay = judgementTime - generatedTime;
                        lanesManager.lanes[noteParam.laneNum].Move(t, delay, noteParam.var_1, noteParam.connectionType);
                        chart.lanePosIndexList[noteParam.laneNum].RemoveAt(0);
                    }
                    break;
                case ElementType.LaneWid:
                    if (chart.laneWidIndexList[noteParam.laneNum].Count > 1)
                    {
                        float t = chart.timeData[chart.laneWidIndexList[noteParam.laneNum][1]] - chart.timeData[chart.laneWidIndexList[noteParam.laneNum][0]];
                        float delay = judgementTime - generatedTime;
                        lanesManager.lanes[noteParam.laneNum].Scale(t, delay, noteParam.var_1, noteParam.connectionType);
                        chart.laneWidIndexList[noteParam.laneNum].RemoveAt(0);
                    }
                    break;
            }
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Init()
        {
            timeDataIndex = 0;
            slideNoteIndex = 0;
            speed = chart.initSpeed;
        }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Audio;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノーツを生成・管理するクラス
    /// </summary>
    public class NotesManager
    {
        #region Field
        /// <summary> 生成時間のリストのインデックス </summary>
        private int timeDataIndex = 0;
        /// <summary> スライドノートのリストのインデックス </summary>
        private int slideNoteIndex = 0;
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
        #endregion

        #region Constructor
        /// <param name="chart"> 譜面クラスのインスタンス </param>
        /// <param name="movementManager"> MovementManager のインスタンス </param>
        /// <param name="judgementManager"> JudgementManager のインスタンス </param>
        public NotesManager(Chart chart, MovementManager movementManager, JudgementManager judgementManager)
        {
            this.chart = chart;
            this.speed = chart.initSpeed;
            this.movementManager = movementManager;
            this.judgementManager = judgementManager;
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
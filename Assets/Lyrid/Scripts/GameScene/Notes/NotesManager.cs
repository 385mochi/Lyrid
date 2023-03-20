using System.Collections;
using System.Collections.Generic;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Audio;

namespace Lyrid.GameScene.Notes
{
    // ノーツを生成・管理するクラス
    public class NotesManager
    {
        #region Field
        // 生成時間やスライドノートのリストのインデックス
        private int timeDataIndex = 0;
        private int slideNoteIndex = 0;
        // ノートの落下速度
        private float speed;
        // レーンの高さ
        private float laneHeight;
        // 譜面のインスタンス
        private Chart chart;
        // MovementManager のインスタンス
        private MovementManager movementManager;
        // JudgementManager のインスタンス
        private JudgementManager judgementManager;
        #endregion

        #region Constructor
        public NotesManager(Chart ch, MovementManager movementManager, JudgementManager judgementManager)
        {
            chart = ch;
            speed = chart.initSpeed;
            this.movementManager = movementManager;
            this.judgementManager = judgementManager;
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
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

        // index 番目のノーツを生成するメソッド
        private void GenerateNote(float time, int index)
        {
            float generatedTime = time;
            float judgementTime = chart.timeData[index];
            NoteParam noteParam = chart.notesData[index];
            // ノーツを生成
            switch (noteParam.type)
            {
                case ElementType.Tap:
                    TapNote tapNote = new TapNote(generatedTime, judgementTime, noteParam);
                    movementManager.AddTarget(tapNote);
                    judgementManager.AddTarget(tapNote);
                    break;
            }
        }

        // 初期化メソッド
        public void Reset()
        {
            timeDataIndex = 0;
            slideNoteIndex = 0;
            speed = chart.initSpeed;
        }
        #endregion
    }
}
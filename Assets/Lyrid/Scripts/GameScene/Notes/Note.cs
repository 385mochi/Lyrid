using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Consts = Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Notes
{
    // ノートの抽象クラス
    public abstract class Note : IMovable, IJudgeable
    {
        #region Field
        // 生成時間
        protected float generatedTime;
        // 判定時間
        public float judgementTime;
        // (判定時間 - 生成時間) の逆数
        protected float inverseTime;
        // 判定の猶予座標
        private float margin = 0.2f;
        // 判定されたかどうか
        public bool judged = false;
        // スライドノートの構成要素かどうか
        private bool isSlideNote = false;
        // ノートの View
        protected NoteView noteView;
        #endregion

        #region Constructor
        public Note(float generatedTime, float judgementTime, NoteParam noteParam)
        {
            // 各時間を取得
            this.generatedTime = generatedTime;
            this.judgementTime = judgementTime;
            inverseTime = 1.0f / (judgementTime - generatedTime);
            // オブジェクトを生成し、初期化
            GameObject noteObj = GameObject.FindWithTag("ObjectPool").GetComponent<ObjectPool>().GetObject();
            noteView = noteObj.GetComponent<NoteView>();
            noteView.Reset(noteParam);
        }
        #endregion

        #region Methods
        public abstract void Move(float rate);
        public abstract JudgementType Judge(float time, int touchType, float posX);

        // View を削除するメソッド
        public void Remove()
        {
            noteView.Remove();
        }

        // 差分時間から判定を取得するメソッド
        protected JudgementType GetJudgement(float diffTime)
        {
            if (diffTime < -Consts.BAD_RANGE)
            {
                return JudgementType.None;
            }
            else if (Consts.BAD_RANGE < diffTime)
            {
                return JudgementType.Miss;
            }
            else if (-Consts.PERFECT_RANGE < diffTime && diffTime < Consts.PERFECT_RANGE)
            {
                return JudgementType.Perfect;
            }
            else if (-Consts.GREAT_RANGE < diffTime && diffTime < Consts.GREAT_RANGE)
            {
                return JudgementType.Great;
            }
            else if (-Consts.GOOD_RANGE < diffTime && diffTime < Consts.GOOD_RANGE)
            {
                return JudgementType.Good;
            }
            else
            {
                return JudgementType.Bad;
            }
        }

        // posX がノートの範囲内かどうか判定するメソッド
        protected bool Touched(float posX)
        {
            return noteView.Touched(posX, margin);
        }
        #endregion
    }
}
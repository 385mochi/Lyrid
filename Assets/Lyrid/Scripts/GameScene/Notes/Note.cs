using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

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
        // レーン番号
        private int laneNum;
        // ノート幅
        private float NoteWidth;
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
        public abstract JudgementType Judge();
        public void Remove()
        {
            noteView.Remove();
        }
        #endregion
    }
}
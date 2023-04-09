using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using static Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノートの抽象クラス
    /// </summary>
    public abstract class Note : IMovable, IJudgeable
    {
        #region Field
        /// <summary> (判定時間 - 生成時間) の逆数 </summary>
        protected float inverseTime;
        /// <summary> スライドノートかどうか </summary>
        private bool isSlideNote;
        #endregion

        #region Property
        /// <summary> 判定時間 </summary>
        public float judgementTime { get; private set; }
        /// <summary> ノートの View </summary>
        public NoteView view { get; protected set; }
        /// <summary> 判定されたかどうか </summary>
        public bool judged { get; set; }
        #endregion

        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートのパラメータ </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public Note(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
        {
            // 各時間を取得
            this.judgementTime = judgementTime;
            this.isSlideNote = isSlideNote;
            inverseTime = 1.0f / (judgementTime - generatedTime);
            // オブジェクトを生成し、初期化
            GameObject noteObj = GameObject.FindWithTag("NoteObjectPool").GetComponent<ObjectPool>().GetObject();
            view = noteObj.GetComponent<NoteView>();
            view.Init(generatedTime, judgementTime, noteParam, isSlideNote);
            judged = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// ノートを移動するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <returns> 以降 Move を実行するかどうか </returns>
        public bool Move(float time)
        {
            // ノーツの位置を更新する
            view.Move((judgementTime - time) * inverseTime);
            if (isSlideNote)
            {
                return view.gameObject.activeSelf;
            }
            else
            {
                return !judged;
            }
        }

        /// <summary>
        /// ノートを判定する抽象メソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチの x 座標 </param>
        /// <returns> 判定の種類 </returns>
        public abstract JudgementType Judge(float time, int touchType, float posX);

        /// <summary>
        /// タッチ位置がノートの範囲内かどうか判定するメソッド
        /// </summary>
        /// <param name="posX"> タッチ位置の x 座標 </param>
        /// <returns> ノートの範囲内かどうか </returns>
        public bool Touched(float posX)
        {
            return view.Touched(posX);
        }

        /// <summary>
        /// View を削除するメソッド
        /// </summary>
        protected void Remove()
        {
            if (isSlideNote)
            {
                view.SetColor(ElementType.None);
            }
            else
            {
                view.Remove();
            }
        }

        /// <summary>
        /// 差分時間から判定を取得するメソッド
        /// </summary>
        /// <param name="diffTime"> 差分時間 </param>
        /// <returns> 判定時間 </returns>
        protected JudgementType GetJudgement(float diffTime)
        {
            if (diffTime < -BAD_RANGE)
            {
                return JudgementType.None;
            }
            else if (BAD_RANGE < diffTime)
            {
                return JudgementType.Miss;
            }
            else if (-PERFECT_RANGE < diffTime && diffTime < PERFECT_RANGE)
            {
                return JudgementType.Perfect;
            }
            else if (-GREAT_RANGE < diffTime && diffTime < GREAT_RANGE)
            {
                return JudgementType.Great;
            }
            else if (-GOOD_RANGE < diffTime && diffTime < GOOD_RANGE)
            {
                return JudgementType.Good;
            }
            else
            {
                return JudgementType.Bad;
            }
        }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// フリックノートのクラス
    /// </summary>
    public class FlickNote : Note
    {
        #region Field
        /// <summary> 初回タッチされたかどうか </summary>
        private bool touched = false;
        /// <summary>  初回タッチの判定 </summary>
        private JudgementType firstJudgementType = JudgementType.None;
        #endregion

        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートのパラメータ </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public FlickNote(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
            : base(generatedTime, judgementTime, noteParam, isSlideNote)
        {}
        #endregion

        #region Methods
        /// <summary>
        /// ノートを移動するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <returns> 以降 Move を実行するかどうか </returns>
        public override bool Move(float time)
        {
            // 差分時間
            float diffTime = time - judgementTime;
            // ノーツの位置を更新する
            view.Move((-diffTime) * inverseTime);
            // Miss 判定ならば判定済みとする
            if (GetJudgement(diffTime) == JudgementType.Miss)
            {
                Remove();
                judged = true;
            }
            return view.gameObject.activeSelf;
        }

        /// <summary>
        /// ノートを判定するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチの x 座標 </param>
        /// <returns> 判定の種類 </returns>
        public override JudgementType Judge(float time, int touchType, float posX)
        {
            // 差分時間
            float diffTime = time - judgementTime;
            // 差分時間から判定を取得
            JudgementType judgementType = GetJudgement(diffTime);
            // 判定済みであれば Judged を返す
            if (judged)
            {
                return JudgementType.Judged;
            }
            // None であればそのまま返す
            if (judgementType == JudgementType.None)
            {
                return JudgementType.None;
            }
            // タッチされていない状態で、タッチ開始かつノートの範囲内であればタッチされた状態にする
            // また、今回の判定を記録しておく
            if (!touched && touchType == 1 && Touched(posX))
            {
                Remove();
                touched = true;
                firstJudgementType = judgementType;
                return JudgementType.None;
            }
            // 既にタッチされた状態で、タッチが移動中であれば初回タッチ時の判定を返す
            if (touched && touchType == 2)
            {
                Remove();
                judged = true;
                return firstJudgementType;
            }
            return JudgementType.None;
        }
        #endregion
    }
}
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

            // 判定済み、または判定が None であれば None を返す
            if (judged || judgementType == JudgementType.None)
            {
               return JudgementType.None;
            }

            // touchType が 0 かつ Miss 判定であれば Miss を返す
            if (touchType == 0 && judgementType == JudgementType.Miss)
            {
                Remove();
                judged = true;
                return JudgementType.Miss;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// タップノートのクラス
    /// </summary>
    public class TapNote : Note
    {
        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートの種類 </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public TapNote(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
            : base(generatedTime, judgementTime, noteParam, isSlideNote)
        {}
        #endregion

        #region Methods
        /// <summary>
        /// ノートを判定するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチ位置の x 座標 </param>
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

            // Miss 判定であれば Miss を返す
            if (judgementType == JudgementType.Miss)
            {
                Remove();
                judged = true;
                return JudgementType.Miss;
            }

            // タッチ開始かつノートの範囲内であれば判定済みとしてその判定を返す
            if (touchType == 1 && Touched(posX))
            {
                Remove();
                judged = true;
                return judgementType;
            }

            return JudgementType.None;
        }
        #endregion
    }
}
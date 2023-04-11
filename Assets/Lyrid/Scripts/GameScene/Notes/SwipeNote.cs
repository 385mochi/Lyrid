using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// スワイプノートのクラス
    /// </summary>
    public class SwipeNote : Note
    {
        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートのパラメータ </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public SwipeNote(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
            : base(generatedTime, judgementTime, noteParam, isSlideNote)
        {}
        #endregion

        #region Methods
        /// <summary>
        /// ノートを判定するメソッド
        /// </summary>
        /// <param name="time"> 判定時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチ位置の x 座標 </param>
        /// <returns> 判定時間 </returns>
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

            // スライドノートの要素であり、タッチ終了時判定時間が負の値で、ノートの範囲内であればそのときの判定を返す
            if (isSlideNote && touchType == 4 && (diffTime < 0) && Touched(posX))
            {
                Remove();
                judged = true;
                return judgementType;
            }

            // タッチ開始 or 途中であり、判定時間が正の値で、ノートの範囲内であれば判定する
            if ((touchType == 1 || touchType == 2 || touchType == 3 || touchType == 4) && (diffTime >= -0.008f) && Touched(posX))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Consts = Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Notes
{
    // タップノートのクラス
    public class TapNote : Note
    {
        #region Field
        #endregion

        #region Constructor
        public TapNote(float generatedTime, float judgementTime, NoteParam noteParam)
            : base(generatedTime, judgementTime, noteParam)
        {
        }
        #endregion

        #region Methods
        // ノーツを移動するメソッド
        public override void Move(float time)
        {
            float rate = (judgementTime - time) * inverseTime;
            noteView.Move(rate);
        }

        // ノートを判定するメソッド
        public override JudgementType Judge(float time, int touchType, float posX)
        {
            // 判定時間との差分を取得
            float diffTime = time - judgementTime;
            // 差分時間から判定を取得
            JudgementType judgementType = GetJudgement(diffTime);
            // 判定済みであれば None を返す
            if (judged)
            {
                return JudgementType.None;
            }
            // None または Miss 判定であればそのまま返す
            if (judgementType == JudgementType.None || judgementType == JudgementType.Miss)
            {
                return judgementType;
            }
            // タッチ開始かつノートの範囲内であれば判定する
            if (touchType == 1 && Touched(posX))
            {
                return judgementType;
            }
            return JudgementType.None;
        }
        #endregion
    }
}
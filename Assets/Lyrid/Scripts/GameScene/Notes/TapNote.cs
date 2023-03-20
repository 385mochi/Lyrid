using System.Collections;
using System.Collections.Generic;
using Lyrid.GameScene.Charts;
using UnityEngine;

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
        public override void Move(float time)
        {
            float rate = (judgementTime - time) * inverseTime;
            noteView.Move(rate);
        }
        public override JudgementType Judge()
        {
            return JudgementType.None;
        }
        #endregion
    }
}
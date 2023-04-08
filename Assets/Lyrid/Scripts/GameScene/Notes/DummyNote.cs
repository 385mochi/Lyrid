using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ダミーノートのクラス
    /// </summary>
    public class DummyNote : Note
    {
        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートのパラメータ </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public DummyNote(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
            : base(generatedTime, judgementTime, noteParam, isSlideNote)
        {}
        #endregion

        #region Methods
        /// <summary>
        /// ノーツを移動するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <returns> 以降 Move を実行するかどうか </returns>
        public override bool Move(float time)
        {
            view.Move((judgementTime - time) * inverseTime);
            return view.gameObject.activeSelf;
        }

        /// <summary>
        /// ノートを判定するメソッド(常に判定しない)
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチの x 座標 </param>
        /// <returns> 判定の種類 </returns>
        public override JudgementType Judge(float time, int touchType, float posX)
        {
            // 常に判定しない
            return JudgementType.None;
        }
        #endregion
    }
}
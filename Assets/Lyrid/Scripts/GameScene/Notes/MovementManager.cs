using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノートの移動を管理するクラス
    /// </summary>
    public class MovementManager
    {
        #region Field
        /// <summary> 移動対象の通常ノートのリスト </summary>
        private List<Note> targetNoteList;
        /// <summary> 移動対象の通常ノートのリスト </summary>
        private List<SlideNote> targetSlideNoteList;
        #endregion

        #region Constructor
        public MovementManager()
        {
            targetNoteList = new List<Note>(30);
            targetSlideNoteList = new List<SlideNote>(20);
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        public void ManagedUpdate(float time)
        {
            for (int i = 0; i < targetNoteList.Count; i++)
            {
                if (!targetNoteList[i].Move(time))
                {
                    targetNoteList.RemoveAt(i);
                }
            }
            for (int i = 0; i < targetSlideNoteList.Count; i++)
            {
                if (!targetSlideNoteList[i].Move(time))
                {
                    targetSlideNoteList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            targetNoteList = new List<Note>(30);
            targetSlideNoteList = new List<SlideNote>(20);
        }

        /// <summary>
        /// 移動対象を追加するメソッド (通常ノート)
        /// </summary>
        /// <param name="target"> 移動対象 </param>
        public void AddTarget(Note target)
        {
            targetNoteList.Add(target);
        }

        /// <summary>
        /// 移動対象を追加するメソッド (スライドノート)
        /// </summary>
        /// <param name="target"> 移動対象 </param>
        public void AddTarget(SlideNote target)
        {
            targetSlideNoteList.Add(target);
        }
        #endregion
    }
}
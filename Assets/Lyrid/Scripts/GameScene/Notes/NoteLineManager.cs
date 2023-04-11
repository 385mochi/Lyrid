using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 同時押しラインを管理するクラス
    /// </summary>
    public class NoteLineManager
    {
        #region Field
        /// <summary> 同時押しラインのリスト </summary>
        private List<NoteLine> noteLineList;
        #endregion

        #region Constructor
        public NoteLineManager()
        {
            noteLineList = new List<NoteLine>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        public void ManagedUpdate()
        {
            for (int i = noteLineList.Count - 1; i >= 0; i--)
            {
                // 同時押しラインを更新
                NoteLine noteLine = noteLineList[i];
                noteLine.UpdateLine();

                // ライン上のノートがなくなったらラインを削除
                if (noteLine.noteNum == 0)
                {
                    noteLineList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Reset()
        {
            noteLineList = new List<NoteLine>();
        }

        /// <summary>
        /// 同時押し対象のノートを追加するメソッド
        /// </summary>
        /// <param name="note"> 対象となるノート </param>
        public void AddNote(Note note)
        {
            // 判定時間が同じものがあれば追加
            for (int i = 0; i < noteLineList.Count; i++)
            {
                if (note.judgementTime == noteLineList[i].judgementTime)
                {
                    noteLineList[i].AddNote(note);
                    return;
                }
            }

            // 判定時間が同じものが無い場合は、新たに作成
            NoteLine noteLine = new NoteLine(note.judgementTime);
            noteLine.AddNote(note);
            noteLineList.Add(noteLine);
        }
        #endregion
    }
}